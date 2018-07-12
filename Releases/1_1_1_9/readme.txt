ReadMe File
ADPinger v1.0.0.0 
Created by: Jacob Morrison
Contributors: Justin Saylor & Jonathan Williams
Copyright NEJ Inc 2014
Triple-J-Coding Production
adpinger@jakemorrison.name
http://techthoughts.jakemorrison.name/adpinger


CONTENTS
I. Description / Features
II. Minimum System Requirements
III. Installation / Operation
IV. Known Issues


I. DESCRIPTION / FEATURES
ADPinger is a VB.NET program designed in Visual Studio 2012 and tailored to the .NET 4.0 runtime environment.  
It is capable of rapidly performing various functions on a list of devices the user specifies.
A device list can be populated by selecting an Active Directory OU containing computer objects, or by selecting a .txt/.csv file that has been pre-populated with a list of device names.

**FUNCTIONS**
--PING
Rapidly sends ICMP requests to all identified objects.
Generate an overall health status of an OU / Container / Device list after a scan.
It can additionally save the results to a CSV report for more robust data manipulation over time.
ADPinger is capable of returning 3 various states of ICMP response:
i. Success - the object is actively pinging
ii. Timed Out - the object has a DNS entry that resolved, but it is not currently pinging
(this status typically indicates an object that is still active in the environment but is currently powered down)
iii. Unknown Host - the object did not resolve against DNS
(This status typically indicates an object is no longer in the environment)

--REBOOT
Sends a reboot command (with user specified options) to all identified objects.
Reboot an entire OU, or a list of devices.
Provide a time to reboot to give users the opportunity to save their work.
Provide a custom message to the user (ie This PC will go down for maintenance in 2 minutes)
Force reboot - will close any open applications / hung applications.

--Shutdown
Sends a shutdown command (with user specified options) to all identified objects.
Shutdown an entire OU, or a list of devices.
Provide a time to shutdown to give users the opportunity to save their work.
Provide a custom message to the user (ie This PC will go down for maintenance in 2 minutes)
Force shutdown - will close any open applications / hung applications.

--SCAN FOR FILE
Scans all identified objects for a local file path that the user specifies.
If file is found, it will be listed on the end report as:
i. File Found - file was located
ii. File Not Found - file was not found in the specified location
iii. Unable to determine - unable to access the device - most likely a permissions issue

--SCAN FOR DIRECTORY
Scans all identified objects for a local directory path that the user specifies.
If directory is found, it will be listed on the end report as:
i. File Found - directory was located
ii. Directory Not Found - directory was not found in the specified location
iii. Unable to determine - unable to access the device - most likely a permissions issue

--COPY FILE
User specifies a source path relative to the device performing the copy. This path can be manually specified or populated by selecting it via windows explorer.
User specifies a destination path relative to the remote device where the file will be copied too.
Copies file from source to relative destination path for all devices specified.
If the relative destination path does not exist and the Create Dir? box is not checked, the copy function will fail.
If the relative destination path does not exist and the Create Dir? box is checked, the destination path specified will be created on the remote device.

--DELETE FILE
User specifies a file path relative to the remote device.
Deletion of the file will be performed on all identified objects.
It is important to note this action is not reversible as the file selected will not be placed inside the recycle-bin of the remote device.


II. MINIMUM SYSTEM REQUIREMENTS
-Windows OS (XP or higher)
-.NET 4.0 runtime environment
-ICMP must be permitted on the network
-Client’s firewall must be configured to accept/respond to ICMP
-Client’s firewall must be configured to accept file sharing requests (File Functions – Scan, Copy, Delete)
-Minimum 1024x768 screen resolution
-Actions require ADPinger to be run with credentials that are present in the Local Administrators group for each device
-For Active Directory integration and performing actions against entire OU's:
--PC running ADPinger must be joined to the domain you are attempting to interact with
--PC running ADPinger should be run from a user profile with at least AD Account Operator credentials (recommended - in some environments this will not be required)
-1GB/s NIC card (recommended)



III. INSTALLATION / OPERATION
ADPinger is a standalone executable and does not require an installation.
To run, execute ADPinger.exe.
1. Select an Action
-There are 6 possible actions to choose from: Ping, Reboot, Scan for file, Scan for directory, Copy File, Delete File
--Choose the desired action - this will open up 2. Options and 3. List Location
2. Options
-Depending on the action selected, there will be various options to choose from.
--Ping
---Time-Out (in milliseconds) - The timeout can be adjusted for Ping (ICMP requests). This can be adjusted to your needs. 1000 is recommended for most situations
--Reboot
---Reboot Message (optional) - Display a custom message to the end user during the reboot process.
---Time (seconds) - The amount of time before the restart will begin - recommended that you give the user some time to save their work, and to read the custom message
---Force Reboot - if checked this will kill all running tasks/hung tasks and force the reboot
--Shutdown
---Shutdown Message (optional) - Display a custom message to the end user during the shutdown process.
---Time (seconds) - The amount of time before the shutdown will begin - recommended that you give the user some time to save their work, and to read the custom message
---Force Shutdown - if checked this will kill all running tasks/hung tasks and force the reboot
--Scan for file
---Enter full path for the file - this file path is relative to the remote device.  So, if you want to see if device name: [Test-Device] has the File.txt file inside the c:\temp folder then enter: C:\Temp\File.txt. ADPinger will scan all identified devices and check each one for the C:\Temp\File.txt and return the results.
--Scan for directory
---Enter full directory path - this directory path is relative to the remote device. So, if you want to see if a device name: [Test-Device] has the directory c:\temp then enter: c:\temp.  ADPinger will scan all identified devices and check each one for the c:\temp directory and return the results.
--Copy File
---Source Path - this is the path to the file that will be copied - this path is relative to the device that ADPinger is running *ON*. The file path can be manually entered or selected by navigating to the file via windows explorer
---Destination Path - this is the path that the file will be copied too - this path is relative to the remote device. The filename *can* be changed on the destination path.  For instance, copying c:\temp\file1.txt to destination c:\temp\file2.txt is permitted.  The file will remain the same it will just have a different name.  Keep in mind OS specific paths.  ADPinger cannot currently differentiate OS type on the remote device.  (ie C:\Users\Public\Desktop is great when copying to Windows 7, but does nothing for Windows XP)
---Create Dir?  This checkbox when activated will create the folder path if it does not currently exist.  If it is not checked, and the folder path does not exist, then the file will not be copied.  It will be copied if the existing file path is present.  It is recommended that this box not be checked when utilizing OS Specific folder paths (ie C:\Users\Public\Desktop)
--Delete File
---Enter full path for the file - this file path is relative to the remote device.  This file will be deleted from all identified objects.  This action is not possible to reverse, so be sure that you want to permanently delete the file before beginning.
3. List Location
ADPinger can process devices located in an OU/container or from a file containing a list of device names
--Load from AD
If ADPinger recognizes a domain the domain name will be displayed in green under the Load from AD option.  If ADPinger cannot detect a domain then this option will not be enabled and you will see a red Domain not detected under this option.
If the Load from AD option is available, selecting it will display a list of the domain's containers and OU's.
Select the desired container/OU and ADPinger will perform the desired action against all devices located within that OU.
--Load from File
You can load either a .txt or .csv that contains a properly formatted list of computer names.
If successful you will see a green File Loaded. Click Begin when ready displayed underneath this option.
Proper formatting for PC Names is one name per line:
PCName1
PCName2
PCName3
no extra spaces, no extra carriage returns

Begin button:
Clicking begin will initiate the action you have selected against the devices that you have specified.
Reset button:
All settings reset


IV. KNOWN ISSUES
** PLEASE SUBMIT ANY ADDITIONAL BUGS FOUND TO: adpinger@jakemorrison.name **
-ADPinger is currently limited to scanning OU's/containers/list of devices with less than 9,999 objects.  
-ADPinger has been shown to run successfully even while being operated with normal user credentials 
(users that do not have any administrative rights to the Active Directory environment).  However, to ensure stability it is recommended that ADPinger be run from a user profile that has at least AD Account Operator rights.
-ADPinger achieves its rapid ping capabilities by utilizing hundreds of threads.  This is beneficial 
in that thousands of objects can be pinged/scanned in only a few minutes but it makes it quite complex to include a cancel feature as each thread must be individually terminated.  Currently, ADPinger does not support a cancellation option and must complete a scan/ping cycle once initiated.
-ADPinger is not currently equipped to handle OS specific paths when utilizing the copy feature.  For example, if you wanted to copy a file to c:\users\public\desktop ADPinger will do great when processing a list of Windows 7 devices.  However, that file path does nothing for XP devices.  It's my goal to address this limitation in a future release.  When processing a mixed list of devices (XP/7) keep this limitation in mind.