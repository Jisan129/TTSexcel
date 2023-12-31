using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net.Http;
using System.Xml.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Media;
using System.Threading;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Drawing;
using TTSexcel.Properties;
using Microsoft.Office.Core;



namespace TTSexcel
{
    [ComVisible(true)]
    public class Ribbon1 : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public string gender = "Male";
        int MAX_RETRY_COUNT = 5;
        int retryCount = 0;
        List<string> stringList = new List<string>();

        string URL = "https://stt.bangla.gov.bd:9381/utils";
        Dictionary<string, string> stringDictionary = new Dictionary<string, string>();
        Dictionary<string, string> downloadDictionary = new Dictionary<string, string>();
        Queue<string> stringQueue = new Queue<string>();
        Queue<string> downloadQueue = new Queue<string>();
     
        private AudioPlayer audioPlayer;
        Excel.Range selectedCell;
        int wordsPerString = 8;
        private bool femaleOn=false;
        private bool maleOn = true;
        private bool startBtnPressed = false;
        private bool highlight = false;
        private bool playing=true;
        private bool downloadFlag=false;
        private int chunkNumbers=0;
        private bool firstPress = true;
        private List<byte[]> byteArrayList = new List<byte[]>();
        private int index=0;
        private bool playPauseLabel = true;
        List<Task> tasks = new List<Task>();
        public object Messagebox { get; private set; }
        private Task allTasksCompleitionTask;
        public bool playPauseUi = true;
        public bool stopFlag = true;
        private bool unicodeButton = true;

        string commonDownloadDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";



        public Ribbon1()
        {
  
        }

        public void StartAudio(Office.IRibbonControl control, bool pressed)
        {

            deleteDirectory();
            createDirectory();
            downloadFlag = false;

            playPauseUi = !playPauseUi;
            ribbon.InvalidateControl("play");
            if (!startBtnPressed)
            {
                ribbon.InvalidateControl("buttonStop");
                Excel.Application excelApp = Globals.ThisAddIn.Application;
                selectedCell = excelApp.Selection as Excel.Range;
                string text = selectedCell.Text;
                text = Bijoy2Uni.Convert(text, Bijoy2Uni.Conversion.All);
                if (firstPress)
                {
                    chunkify(text);
                    firstPress = false;
                    playPauseLabel = false;

                }
                else
                {
                    playPauseLabel=false; 
                    audioPlayer.PlaySync();
                }
            }
            else
            {
                playPauseLabel = true;
                audioPlayer.Pause();
            }

             highlight = false;
             startBtnPressed = !startBtnPressed;
        
        }
        public void deleteDirectory()
        {
            string directoryPath = commonDownloadDirectory + "\\Down";

            try
            {
                // Attempt to delete the directory
                Directory.Delete(directoryPath, true);

                // Directory has been deleted successfully
                Console.WriteLine("Directory deleted successfully.");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during directory deletion
                Console.WriteLine($"Error deleting directory: {ex.Message}");
            }
        }


        public void createDirectory()
        {
            // Specify the path of the directory you want to create
            string directoryPath = commonDownloadDirectory+"\\Down";

            try
            {
                // Attempt to create the directory
                Directory.CreateDirectory(directoryPath);

                // Directory has been created successfully
                Console.WriteLine("Directory created successfully.");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during directory creation
                Console.WriteLine($"Error creating directory: {ex.Message}");
            }
        }
        public void restartData()
        {
            startBtnPressed = false;
            firstPress = true;
            stringDictionary.Clear();
            stringQueue.Clear();
        }
         
        public void ssml(Microsoft.Office.Core.IRibbonControl control)
        {
            audioPlayer.Stop();
            restartData();
            downloadFlag = false;
        }
        public bool getStopButtonVisible(IRibbonControl control)
        {
            if (startBtnPressed)
            {
                return true;

            }
            return false;
        }
        public bool downloadEnable(IRibbonControl control)
        {
            
            if (downloadFlag)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void stopFunction(IRibbonControl control)
        {
            audioPlayer.Stop();
            playPauseLabel = true;
            downloadFlag = false;
            stopFlag = false;
            ribbon.InvalidateControl("play");
            ribbon.InvalidateControl("download");
            ribbon.InvalidateControl("buttonStop");
            restartData();
        }

        public void convertBijoy(Microsoft.Office.Core.IRibbonControl control,bool pressed)
        {
            unicodeButton = false;
            Excel.Application excelApp = Globals.ThisAddIn.Application;
            selectedCell = excelApp.Selection as Excel.Range;
            selectedCell.Font.Name = "SutonnyMJ";



        }
        public void convertUnicode(IRibbonControl control,bool pressed)
        {
            unicodeButton= true;

        }
        public bool getPlayingStatus(IRibbonControl control)
        {
            ribbon.InvalidateControl("speed_box");
            ribbon.InvalidateControl("pitch_box");
            ribbon.InvalidateControl("male");
            ribbon.InvalidateControl("female");
            ribbon.InvalidateControl("uni2bijoy");
            ribbon.InvalidateControl("bijoy2uni");


            if (startBtnPressed)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        public void ClearText(Microsoft.Office.Core.IRibbonControl control)
        {
            stringDictionary.Clear();
            audioPlayer.Pause();
            stringQueue.Clear();
            download();
        }
        public void HighlightTextInCell(Excel.Range cell,string chunk)
        {
            //cell.Characters[startIndex, length].Font.Color = Excel.XlRgbColor.rgbRed;
            // You can replace 'rgbRed' with the color you want to use for highlighting.

            Excel.Range cellCharacters = (Range)cell.Characters[2, 5];
            cellCharacters.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
        }

        private async Task networkCall(string Text)
        {


            if (Text.Length > 3)
            {
                stringQueue.Enqueue(Text);
                downloadQueue.Enqueue(Text);
                var ret = new byte();
                var response = await SendRequest(Text);
                byte[] audioBytes = await response.Content.ReadAsByteArrayAsync();
               // string content = await response.Content.ReadAsStringAsync();
               //JObject jsonObject = JObject.Parse(content);
                //JToken audioToken = jsonObject["output"];
               // string text = (string)audioToken;
               string text= Convert.ToBase64String(audioBytes);


                stringDictionary.Add(Text, text);
                downloadDictionary.Add(Text, text);


            }

        }

        private async Task bridgeAsync(string Text)
        {
      
            await networkCall(Text);

            while (stringQueue.Count > 0)
            {
                string queue = stringQueue.Peek();
                if (stringDictionary.ContainsKey(queue))
                {
                    string value = stringDictionary[queue];
                    stringDictionary.Remove(queue);
                    stringQueue.Dequeue();
                    if (playing)
                    {
                        playAudio(value, queue);

                    }

                }

            }

            downloadFlag = true;
            stopFlag = false;
            playPauseUi = true;
            ribbon.InvalidateControl("play");
            ribbon.InvalidateControl("download");
            ribbon.InvalidateControl("buttonStop");

        }


        public void download2(string base64Audio)
        {


            byte[] audioData = Convert.FromBase64String(base64Audio);

            string outputFilePath = commonDownloadDirectory+"\\down\\output" + index.ToString() + ".wav";
            index++;
            // Write the audio data to a WAV file at the specified output path
            File.WriteAllBytes(outputFilePath, audioData);

            if(index >= chunkNumbers-1)
            {
                MergeWavFiles("abc");

            }

        }
    
        public void deleteFiles()
        {
            for (int i = 0; i < chunkNumbers - 1; i++)
            {
                string filePath = @"C:\Users\revet\Documents\down\output" + i + ".wav";
                if (File.Exists(filePath))
                {
                    Thread.Sleep(1000);
                    Thread thread = new Thread(() => File.Delete(filePath));
                    thread.Start(); 
                    //File.Delete(filePath);
                }
                else
                {
                    Console.WriteLine($"File does not exist: {filePath}");
                }
            }
        }
        
        public void download()
        {
            int i = 0;
            while(chunkNumbers-1 >= i)
            {
                string queue = downloadQueue.Peek();
                if (downloadDictionary.ContainsKey(queue))
                {
                    string value = downloadDictionary[queue];
                    downloadDictionary.Remove(queue);
                    
                    download2(value);
                    downloadQueue.Dequeue();
                    downloadQueue.Enqueue(value);

                }
                i++;
            }
        }
        public void SaveMergedAudio(string filePath)
        {
            // Concatenate all the merged audio data
            byte[] finalAudioData = new byte[0];
            foreach (byte[] audioData in byteArrayList)
            {
                finalAudioData = ConcatenateByteArrays(finalAudioData, audioData);
            }

            // Save the merged audio data to a file (e.g., WAV)
            File.WriteAllBytes(filePath, finalAudioData);
        }



        //ok

        public void MergeWavFiles( string outputFilePath)
        {
            // Create a list of WaveStreams
            List<string> files = new List<string>();
            List<AudioFileReader> readers = new List<AudioFileReader>();

            for (int i = 0; i < chunkNumbers-1; i++)
            {

                files.Add(commonDownloadDirectory + "\\down\\output" + i + ".wav");
            }
            List<WaveStream> filePaths = new List<WaveStream>();
            
            for(int i=0; i < chunkNumbers-1; i++)
            {
                var reader= new AudioFileReader(files[i]);
                readers.Add(reader);
            }
  
 
            var playlist = new ConcatenatingSampleProvider(readers);
            
            WaveFileWriter.CreateWaveFile16(commonDownloadDirectory + "\\output_final_download.wav",playlist);
            deleteDirectory();
           // Thread.Sleep(5000);
           // string directoryPath = (@"C:\Users\revet\Documents\down");
          //  Directory.Delete(directoryPath, true);


           // deleteFiles();
        }
        private byte[] ConcatenateByteArrays(byte[] firstArray, byte[] secondArray)
    {
        byte[] result = new byte[firstArray.Length + secondArray.Length];
        Buffer.BlockCopy(firstArray, 0, result, 0, firstArray.Length);
        Buffer.BlockCopy(secondArray, 0, result, firstArray.Length, secondArray.Length);
        return result;
    }

        public void setGender(Office.IRibbonControl control, bool pressed)
        {
            femaleOn = !femaleOn;
            if (femaleOn)
            {
                gender = "Female";
                maleOn = false;
                ribbon.InvalidateControl("male");
           
            }
            else
            {
                gender = "Male";
            }
            ribbon.InvalidateControl("female");
        }  
        public void setGender2(Office.IRibbonControl control, bool pressed)
        {
            maleOn = !maleOn;
            if (maleOn)
            {
                gender = "Male";
                femaleOn = false;
                ribbon.InvalidateControl("female");

            }
            else
            {
                gender = "Female";
            }
            ribbon.InvalidateControl("male");
        }


        //Helper functions

        async void chunkify(String text)
        {
           
            string[] segments = SplitStringByPunctuation(text, new char[] { ',', '.', '?', '|', ':', ';', '!', '।','\n' });
            chunkNumbers = segments.Length;
            List<string> substringList = new List<string> { };
            foreach (string segment in segments)
            {

                if (countWords(segment) > 20)
                {
                    string[] words = segment.Split(' ');

                    int numStrings = (int)Math.Ceiling((double)words.Length / wordsPerString);

                    for (int i = 0; i < numStrings; i++)
                    {
                        int startIndex = i * wordsPerString;
                        int endIndex = Math.Min(startIndex + wordsPerString, words.Length);

                        string[] substringWords = words.Skip(startIndex).Take(endIndex - startIndex).ToArray();
                        string substring = string.Join(" ", substringWords);
                        substringList.Add(substring);

                    }
                    chunkNumbers++;
                }
                else
                
                {
                    substringList.Add(segment);

                }
            }

            substringList.Add(" ");
            stringDictionary.Clear();
            downloadDictionary.Clear();
            stringQueue.Clear();
            downloadQueue.Clear();
            
            

            foreach (string substring in substringList)
            {
                await bridgeAsync(substring);

                
            }
            restartData();
            playPauseLabel = true;
            downloadFlag = true;
        }
        
        async Task<HttpResponseMessage> SendRequest(string text)
        {
            HttpResponseMessage response = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                while (retryCount < MAX_RETRY_COUNT)
                {
                    try
                    {
                        var json = "{\"text\": \"" + text + "\", \"module\": \"backend_tts\", \"submodule\": \"infer\"}";
                        response = await client.PostAsync(URL, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                        if (response.IsSuccessStatusCode)
                        {
                            break;
                        }
                    }
                    catch (HttpRequestException error)
                    {
                        Console.WriteLine(error.Message);
                    }

                    retryCount++;
                }
            }

            return response;
        }
        public static string[] SplitStringByPunctuation(string input, char[] punctuationChars)
        {
            // Create a regex pattern for the specified punctuation characters
            string pattern = "[" + Regex.Escape(new string(punctuationChars)) + "]+";

            // Use regex to split the input string based on the pattern
            string[] segments = Regex.Split(input, pattern);
            
            return segments;
        }


        static int countWords(string text)
        {
            string[] words = Regex.Split(text, @"\s");
            return words.Length;
        }


        public void playAudio(string Text,string chunk)
        {
            
                string base64Audio = Text;

                // Convert base64 string to bytes
                byte[] audioBytes = Convert.FromBase64String(base64Audio);
                
                if (!highlight)
                {
                    selectedCell.Interior.Color = Excel.XlRgbColor.rgbYellow;
                    highlight = true;
                }

                audioPlayer = new AudioPlayer(audioBytes);
                audioPlayer.PlaySync();

        }

        #region IRibbonExtensibility Members


        public bool UnicodeButtonPressed(IRibbonControl control)
        {
            return unicodeButton;
        }
        public bool AnsiButtonPressed(IRibbonControl control) { return !unicodeButton; }

        //get Images
        public bool maleOnButton(IRibbonControl control)
        {
            return maleOn;
        }
        public bool femaleOnButton(IRibbonControl control)
        {
            return femaleOn;
        }

        public string getPlayPauseButtonLabel(Microsoft.Office.Core.IRibbonControl control)
        {
            if (playPauseLabel)
            {
                return "Play";
            }
            else
            {
                return "Pause";

            }
        }
        public Bitmap getFemaleImage(Microsoft.Office.Core.IRibbonControl control)
        {
            if (!femaleOn)
            {
                return Resources.offButton;

            }
            else
            {
                return Resources.onButton;
            }
        }

        public Bitmap getMaleImage(Microsoft.Office.Core.IRibbonControl control)
        {
            if (!maleOn)
            {
                return Resources.offButton;
            }
            else {
                return Resources.onButton;
            }
        }
        public Bitmap getPlayPauseButtonImage(Microsoft.Office.Core.IRibbonControl control)
        {
            if (playPauseUi)
            {
                return Resources.playButton;

            }
            else { 
                return Resources.pauseButton;
            }
        }
        public Bitmap getDownloadButtonImage(Microsoft.Office.Core.IRibbonControl control)
        {
            return Resources.downloadButton;
        }

        //
        public Bitmap getButtonStop(IRibbonControl control)
        {
            return Resources.stopButton;
        }

        public Bitmap getLogo(IRibbonControl control)
        {
            return Resources.logo;
        }
        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("TTSexcel.Ribbon1.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit https://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}

