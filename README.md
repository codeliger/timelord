# Timelord

## What is it?

Timelord is a time tracking and invoicing application written in C# using Windows Forms.

Here is the timelord workflow:
* Create a task and start it.
* Stop the task.
* Select the task and select create invoice.
* Set the hourly cost of the task and a description of it.
* Generate an invoice.

## How do I use it?

If you are planning to use Timelord you can check the releases page on github: https://github.com/codeliger/timelord/releases
If there are no releases that means timelord is not ready for use and may have bugs.

To run it in Visual Studio you can clone the repository and start the solution file in Visual Studio.
Timelord uses this package: https://github.com/Fody/Costura to embedd depencies.
This allows timelord to run as a single executable and not have any dll files required to run with it.
