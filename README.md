# NanoDNA.GitHubManager
A Library for GitHub API communication and Action Runner management. Allows you to control and develop automation pipelines related to GitHub within a C# Code Base.

# Requirements
- .NET 8 or Later
- Docker is Installed
- GitHub PAT (Personal Access Token)
	- API Communication Scope : ``repo``
	- Ephemeral Runners Scope : ``workflow``

# Installation / Download
This Framework can be installed using NuGet, Downloading the Self-Contained DLL's or Cloning through GitHub

## Install from NuGet
Use the following command to install the Tool. Replace ``<version>`` with the appropriate version using ``0.0.0`` format.

```bash
dotnet add package NanoDNA.GitHubManager --version <version>
```

## Download Self-Contained Builds
Visit the [``Release``](https://github.com/Nano-DNA-Studios/NanoDNA.GitHubManager/releases) Page and Download the Self-Contained Tars for your Target Platform and OS

## Clone and Build
Clone the latest state of the Repo and Build it locally.

```bash
git clone https://github.com/Nano-DNA-Studios/NanoDNA.GitHubManager
cd NanoDNA.GitHubManager
dotnet build
```

# Library Dependencies
The Library relies on the following NuGet packages that are produced inhouse. These libraries can be used in accordance with their respective MIT or Purchased Licenses.

Libraries: 
- [NanoDNA.ProcessRunner](https://github.com/Nano-DNA-Studios/NanoDNA.ProcessRunner) - Dispatches and Manages System and Shell calls on multiple OS's, used for automation.
- [NanoDNA.DockerManager](https://github.com/Nano-DNA-Studios/NanoDNA.DockerManager) - Initializes, Manages and Controls Docker Containers using C#.
# License
Individuals can use the Library under the MIT License

Groups and or Companies consisting of 5 or more people can Contact MrDNAlex through the email ``Mr.DNAlex.2003@gmail.com`` to License the Library for usage. 

# Support
For Additional Support, Contact MrDNAlex through the email : ``Mr.DNAlex.2003@gmail.com``.