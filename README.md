# ADPinger
This is a VB .NET project created as a hobby in 2013. It is no longer actively developed or maintained. ADPinger's purpose is to assist administrators with a variety of different functions across hundreds or even thousands of remote devices.

## Synopsis
ADPinger is a tool for administrators designed to assist in performing various actions on a large number of remote devices. ADPinger can reboot, scan for files, copy files, and delete files on thousands of remote computers rapidly and with minimal network impact.

## Description

ADPinger is a VB.NET program designed in Visual Studio 2012 and tailored to the .NET 4.0 runtime environment.  

### Capabilities

#### Identify old and unused computer accounts in AD

It is capable of generating an overall health status of an OU / Container after a scan.
It can also save the results of a scan to a CSV report for more robust data manipulation and tracking.

ADPinger is capable of returning 3 various states of ICMP response.:
 * **Success** - the object is actively pinging
 * **Timed Out** - the object has a DNS entry that resolved, but it is not currently pinging (this status typically indicates an object that is still active in the environment but is currently powered down)
 * **Unknown Host** - the object did not resolve against DNS (This status typically indicates an object is nor longer in the environment)

#### Ping

Send ICMP requests to thousands of devices.  Generate robust reports and track inactivity over time.  Use this data to identify old and unused computer accounts, or just get an overall health status of an OU.

#### Reboot

Send custom reboot commands to thousands of devices you specify or reboot an entire OU.

#### Shutdown

Shutdown a list of devices or shutdown an entire OU.

#### Scan for file

Look for a file on any remote device you specify. Generate robust reports if devices do/do not have the file you are looking for.

#### Scan for directory

Look for a directory on any remote device you specify. Generate robust reports if devices do/do not have the directory you are looking for.

#### Copy File

Copy a file to thousands of remote devices.  Copy a file to an entire OU.

#### Delete File

Delete a file from thousands of devices.  Delete a file from an entire OU.  (Be careful!)

## Prerequisites

* Windows OS (XP or higher)
* .NET 4.0 runtime environment
* ICMP must be permitted on the network
* Client’s firewall must be configured to accept/respond to ICMP
* Client’s firewall must be configured to accept file sharing requests (File Functions – Scan, Copy, Delete)
* Minimum 1024x768 screen resolution
* Actions require ADPinger to be run with credentials that are present in the Local Administrators group for each device
* For Active Directory integration and performing actions against entire OU's:
  * PC running ADPinger must be joined to the domain you are attempting to interact with
  * PC running ADPinger should be run from a user profile with at least AD Account Operator credentials (recommended - in some environments this will not be required)
* 1GB/s NIC card (recommended)

## How to run

[ADPinger Video Demo](https://youtu.be/dOIZBVNR5dM)

## Notes

Old site documentation: [http://techthoughts.info/adpinger/](http://techthoughts.info/adpinger/)