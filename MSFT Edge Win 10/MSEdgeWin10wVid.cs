// MicrosoftEdge Script Version 20211027
// Better handling of login through video page


using LoginPI.Engine.ScriptBase;
using System;

public class MicrosoftEdge83 : ScriptBase
{

    private void Execute()
    {
        // Define environment variable to use with Workload
        var temp = GetEnvironmentVariable("TEMP");
              
        // Define random integer
        var rand = new Random();
        var PageBrowseTime = rand.Next(20,30); //How long we stay on the starting web page (4-9 seconds)
        var VideoDuration = rand.Next(30,120); //How long we stay on the starting web page (4-9 seconds)
        
        // Download the VSIwebsite.zip from the appliance and unzip in the %temp% folder
        //// clean up existing site
        Wait(seconds:3, showOnScreen:true, onScreenText:"Setting up the Local Website");
        StartTimer("Delete_and_Download");
        if (System.IO.Directory.Exists($"{temp}\\LoginPI\\vsiwebsite"))
        {
            Log("Removing existing website folder");
            RemoveFolder(path: $"{temp}\\LoginPI\\vsiwebsite");
        }
        else
        {
            Log("Project folder does not exist");
        }

        //Grab the website archive and extract it
        CopyFile(KnownFiles.WebSite, $"{temp}\\LoginPI\\vsiwebsite.zip", overwrite: true);
        UnzipFile($"{temp}\\LoginPI\\vsiwebsite.zip", $"{temp}\\LoginPI\\vsiwebsite", overWrite: true);

        if(!(DirectoryExists($"{temp}\\LoginPI\\vsiwebsite")))
        {
            Log("Downloading File");
            CopyFile(KnownFiles.WebSite, $"{temp}\\LoginPI\\vsiwebsite.zip");
            UnzipFile($"{temp}\\LoginPI\\vsiwebsite.zip", $"{temp}\\LoginPI\\vsiwebsite");
        }
        else
        {
            Log("File already exists");
        }
        StopTimer("Delete_and_Download");
        
        // Start Browser
        Wait(seconds:3, showOnScreen:true, onScreenText:"Start Edge");
        StartBrowser();
        var EdgeBrowser = FindWindow(className : "Win32 Window:Chrome_WidgetWin_1", title : "*Microsoft​ Edge", processName : "msedge");
        EdgeBrowser.Focus();
        EdgeBrowser.Maximize();
        MouseDown();
        MouseUp();
        MainWindow.Type("{PAGEDOWN}".Repeat(2));
        MainWindow.Type("{PAGEUP}".Repeat(1));
        Wait(PageBrowseTime);
/*        
        // Navigate web
        Wait(seconds:3, showOnScreen:true, onScreenText:"NPR");
        Navigate("https://apple.com");
        Wait(RandomNumber);
        Wait(RandomNumber);
*/
        // Navigate to the local html file
        Wait(seconds:3, showOnScreen:true, onScreenText:"Navigating to Local Website");
        Navigate($"file:///{temp}/LoginPI/vsiwebsite/chromescript/logonpage.html");
        Wait(10);

        // Click on the login button
        // Browser.FindWebComponentBySelector("button[id='logonbutton']").Click();
        Wait(seconds:3, showOnScreen:true, onScreenText:"Click Logon Button");
        var LoginBtn=MainWindow.FindControl(className : "Button", title : "Login");
        LoginBtn.Click();
        Wait(1);
        //Type("{TAB}");
        //Wait(1);
        //Type("{SPACE}");
        //Wait(3);

        // Enter login credentials
        var UsernameField=MainWindow.FindControl(className : "Edit", title : "Enter Username");
        UsernameField.Click();
        UsernameField.Type("Admin");
        Wait(1);
        var PasswordField=MainWindow.FindControl(className : "Edit", title : "Enter Password");
        PasswordField.Click();
        PasswordField.Type("Admin");
        //Browser.FindWebComponentBySelector("input[id='username']").Click();
        //Type("Admin");
        //Browser.FindWebComponentBySelector("input[id='password']").Click();
        //Type("Admin");
        var LogonBtn=MainWindow.FindControl(className : "Button", title : "Logon");
        LogonBtn.Click();
        //Browser.FindWebComponentBySelector("button[id='submit']").Click();
        
        // Time the logon
        StartTimer("Logon");
        Browser.FindWebComponentBySelector("a[id='videopage']", timeout: 30, continueOnError: false);
        StopTimer("Logon");
        Wait(1);

        // Select the videopage tab
        Wait(3, showOnScreen: true, onScreenText: $"Watch Platte River for {VideoDuration} seconds");
        Browser.FindWebComponentBySelector("a[id='videopage']").Click();

        // Watch video for VideoDuration seconds
        Wait(VideoDuration);

        // Navigate back to main homepage and Click on Article
        //MainWindow.FindControl(className : "Button:ToolbarButton", title : "Back").Click();
        // The following works with Edge to go back
        MainWindow.FindControl(className : "Button:BackForwardButton", title : "Back").Click();
        Wait(2);
        Browser.FindWebComponentBySelector("a[id='articlepage']").Click();
        Wait(2);

        // Scroll through webpage
        Wait(seconds:3, showOnScreen:true, onScreenText:"Browse a Web Page");
        MainWindow.MoveMouseToCenter();
        MouseDown();
        MouseUp();
        MainWindow.Type("{PAGEDOWN}".Repeat(2));
        MainWindow.Type("{PAGEUP}".Repeat(2));

        // Stop the browser
        Wait(seconds:3, showOnScreen:true, onScreenText:"Stopping Browser");
        Wait(2);
        StopBrowser();

    }
}