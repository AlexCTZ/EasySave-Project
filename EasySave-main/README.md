# EasySave :computer: 
A backup software project created by CESI students :tada: 

## Context

For this project, we are a 5 developers team and we have just joined ProSoft, a software publisher. And so, we have to complete the "EasySave" project: a backup software.

The unit price of the software should be €200 excluding VAT.

We'll need to produce user documentation in English (one-page user manual), customer support (information needed for technical support: default software location, minimum configuration, location of configuration files, etc.), and release notes for each new version of EasySave.

## Documentation

User documentation is available in the [docs](https://github.com/bapt1508/EasySave/tree/main/Doc) folder of our project.

## Version 1.0

The software is a console application using .Net Core and can create up to 5 backup jobs.
The software must be usable by both English and French speaking users.
A backup job is defined by
- A backup name
- A source directory
- A target directory
- A type (full, differential)

The user can request the execution of a single backup job, or the sequential execution of all jobs. The program can be launched from a command line.

**Daily log file:**
The software writes the history of backup job actions in real time to a daily log file. The information it stores is:
- Time stamp
- Backup name
- Source file address (UNC format)
- Address of destination file (UNC format)
- File size
- File transfer time in ms (negative if error)

**Real-time status file:**
The software records the progress of backup jobs in real time, in a single file. The information recorded for each backup job is:
- Name of backup job
- Time stamp
- Backup job status (e.g. Active, Not Active...)

If the job is active:
- Total number of eligible files
- Size of files to be transferred
- Progress
- Number of files remaining
- Size of remaining files
- Full address of source file being backed up
- Full address of destination file

The locations of the two files (daily log and real-time status) can run on the client servers. The files (log and status) and any configuration files are in JSON format.

## Version 2.0

**Graphic Interface**
The application must now be developed in WPF under .Net Core.

**Unlimited number of jobs**
The number of backup jobs is now unlimited. 

**Encryption via CryptoSoft software**
The software is able to encrypt the files using CryptoSoft software.

**Evolution of the Daily Log file**
The daily log file contain additional information: Time needed to encrypt the file (in ms)  

**Business software**
If the presence of business software is detected, the software must prohibit the launch of a backup job. In the case of sequential jobs, the software complete the current job and stop before launching the next job.

## Version 3.0

**Parallel backup**
Backup jobs are performed in parallel.

**No parallel transfer of files larger than n Kb**
To avoid overloading bandwidth, no two files larger than n Kb may be transferred at the same time. (n Kb can be set)

**Real-time interaction with each job or all jobs**
For each backup job (or set of jobs), the user must be able to
- Pause (effective pause after transfer of the current file)
- Play (start or resume a pause)
- Stop (immediate stop of current job)

The user must be able to follow the progress of each job in real time (at least with a progress percentage).

Temporary pause if business software operation is detected
If the software detects the operation of business software, it must pause the backup jobs. Backups restart automatically as soon as the business software is stopped.
The application is single-instance & cannot be launched more than once on the same computer.

## Technologies

The software is written in C# and uses the .NET Core 3.1.
We use Github to ensure proper version control. And for UML diagrams, we use Lucidchart.

## Authors

Baptiste COURTADE | baptiste.courtade@viacesi.fr

Alexandre CARATZA | alexandre.caratza@viacesi.fr

Corentin BEAUFILS | corentin.beaufils@viacesi.fr

Florian DE GRAAF | florian.degraaf@viacesi.fr

Wissal EZZINE | wissal.ezzine@viacesi.fr
