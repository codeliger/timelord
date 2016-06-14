# Timelord
# Open-source time tracking and invoicing software written in C#

## Purpose
The purpose of this project is to create a simple and free time-tracking and invoicing software suite for windows for small time web developers who are not interested in paying monthly to simply track their hours. Most current time tracking solutions are either too complicated for large projects, or web-based and charge monthly fees to simply press start and stop and record seconds. This is unacceptable for independant freelancers or hobbyists who do not make enough to justify these costs.

## Features / Functionality
* Store all task related information in a SQLite Database table
* Time tasks and assign them a description with a background color of red. Red identifies tasks as unpaid and uninvoiced.
* Select entered tasks and generating an invoice which changes the color of those tasks to yellow. Yellow means invoiced.
* Invoice tasks can be selected and marked as paid which turns them to green. Green means invoiced and paid.

## Notice
There is a potential for drastic user interface changes on the main branch. The main intention is to be able to enter task names and time tasks as fast and effeciently as possible so that it doesn't decrease productivity when tabbing back and forth to the application. Many companies want developers to time everything they do, and give them an idea of how long a project is going to take down the the hour. This is hard to accomplish. Future functionality may include a hypothetical project time builder where you drag around previously completed tasks into a list which allows you to plan all of the implementations you need to complete in a project with the associated time it took you.
