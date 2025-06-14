<a id="readme-top"></a>

<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![License][license-shield]][license-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <img src="images/icon.png" alt="CommonUtilities Logo" width="120" height="120">
  <h3 align="center">CommonUtilities</h3>
  <p align="center">
    A modular, production-ready C#/.NET utility library and toolkit for rapid development.<br>
    <a href="#usage"><strong>Explore Usage & Docs »</strong></a>
    <br /><br />
    <a href="#project-structure">Project Structure</a>
    &middot;
    <a href="https://github.com/LoveDoLove/CS_CommonUtilities/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    &middot;
    <a href="https://github.com/LoveDoLove/CS_CommonUtilities/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about-the-project">About The Project</a></li>
    <li><a href="#project-structure">Project Structure</a></li>
    <li><a href="#getting-started">Getting Started</a></li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#funding--support">Funding & Support</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

CommonUtilities is a comprehensive, modular C#/.NET library providing reusable helpers, services, utilities, and UI components for .NET 8+ projects. It is designed for rapid development, maintainability, and best practices, supporting a wide range of common application needs.

### Key Features

- **Console UI**: Menus, prompts, notifications, progress, keyboard shortcuts
- **Helpers**: Console, enums, HTTP, JSON, processes, QR code, scheduling (Cron)
- **Services**: Email, captcha, MFA, image, IP info, Stripe, admin, sync, confirmation
- **Utilities**: Encryption (AES, 3DES, SHA256), validation, logging, file/database, signature, timestamp, caching
- **Models**: Strongly-typed config, user, role, DB, mail, Stripe, response objects
- **Interfaces**: For extensibility and testability
- **Modern .NET project structure**: global usings, nullable enabled, best-practice dependencies

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- PROJECT STRUCTURE -->
## Project Structure

```
.
├── .github/                # GitHub workflows, issue templates, funding config
│   ├── FUNDING.yml
│   ├── ISSUE_TEMPLATE/
│   └── workflows/
├── .vs/                    # Visual Studio local settings (not source-controlled)
├── CommonUtilities/        # Main library source
│   ├── Common/             # Shared logic/constants
│   ├── ConsoleUI/          # Console UI components
│   ├── Helpers/            # Helper classes (Console, Enum, Http, Json, Process, QrCode, Scheduler)
│   ├── Interfaces/         # Service and utility interfaces
│   ├── Models/             # Data models (Config, User, Role, DB, etc.)
│   ├── Services/           # Service implementations (Mailer, Captcha, MFA, etc.)
│   ├── Utilities/          # Utility classes (encryption, validation, logging, etc.)
│   ├── www/                # Static assets (images/icons)
│   ├── GlobalUsings.cs     # Project-wide usings
│   ├── Program.cs          # Entry point (empty for library)
│   ├── CommonUtilities.csproj
│   └── CommonUtilities.csproj.user
├── .gitattributes
├── .gitignore
├── CommonUtilities.sln     # Solution file
├── LICENSE                 # MIT License
├── README.md               # This file
├── README_TEMPLATES.md     # README template and reference
```

### Notable Files & Directories

- **.github/**: Contains [FUNDING.yml](.github/FUNDING.yml) for sponsorship, issue templates, and CI workflows.
- **CommonUtilities/CommonUtilities.csproj**: .NET 8.0 project file, lists all NuGet dependencies (e.g., Serilog, FluentValidation, Stripe, MailKit, MediatR, EntityFrameworkCore, QRCoder, etc.).
- **CommonUtilities/www/images/**: Project icon and static assets.
- **README_TEMPLATES.md**: Reference template for maintaining README quality and structure.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

- [.NET 8 SDK or later](https://dotnet.microsoft.com/download)
- Visual Studio 2022+ or compatible IDE

### Installation

1. **Clone the repository**
   ```sh
   git clone https://github.com/LoveDoLove/CS_CommonUtilities.git
   ```
2. **Open the solution**
   - Open `CommonUtilities.sln` in Visual Studio or your preferred IDE.
3. **Build the solution**
   ```sh
   dotnet build
   ```
4. **Reference the library**
   - Add a project reference to `CommonUtilities/CommonUtilities.csproj` in your .NET project, or copy required modules.

### Directory Reference

- **CommonUtilities/Common/**: Shared constants and logic
- **CommonUtilities/ConsoleUI/**: UI components for console apps
- **CommonUtilities/Helpers/**: Utility helpers (Console, Enum, Http, etc.)
- **CommonUtilities/Interfaces/**: Service and utility interfaces
- **CommonUtilities/Models/**: Data models for config, users, roles, etc.
- **CommonUtilities/Services/**: Service implementations (Mailer, Captcha, etc.)
- **CommonUtilities/Utilities/**: Encryption, validation, logging, etc.
- **CommonUtilities/www/**: Static assets (icons)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE -->
## Usage

Import and use helpers, services, or utilities as needed:

```csharp
using CommonUtilities.Helpers;
using CommonUtilities.Services;
using CommonUtilities.Utilities;

// Example: Using ConsoleHelper
ConsoleHelper.WriteLineSuccess("Operation completed successfully!");

// Example: Using AesUtilities for encryption
string encrypted = AesUtilities.Encrypt("mySecret", "password123");
```

_Refer to the source code in each subfolder for more examples and XML documentation comments._

### Example: Using a Service

```csharp
using CommonUtilities.Services;

IMailerService mailer = new MailerService();
mailer.Send("to@example.com", "Subject", "Body");
```

### Example: Using a Model

```csharp
using CommonUtilities.Models;

var config = new Config { AppName = "MyApp", Version = "1.0" };
```

### Example: Using a Console UI Component

```csharp
using CommonUtilities.ConsoleUI;

MenuBuilder menu = new MenuBuilder();
menu.AddItem("Option 1", () => { /* action */ });
menu.Show();
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTRIBUTING -->
## Contributing

Contributions are welcome! Please review the [README_TEMPLATES.md](README_TEMPLATES.md) for style and structure guidelines.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Issue & PR Templates

- Use the templates in `.github/ISSUE_TEMPLATE/` for bug reports and feature requests.
- CI/CD and workflow files are in `.github/workflows/`.

### Funding

- See [.github/FUNDING.yml](.github/FUNDING.yml) for sponsorship options.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->
## License

Distributed under the MIT License. See [`LICENSE`](LICENSE) for details.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- FUNDING & SUPPORT -->
## Funding & Support

If you find this project useful, consider sponsoring via GitHub Sponsors or other platforms listed in [.github/FUNDING.yml](.github/FUNDING.yml).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [Best README Template](https://github.com/othneildrew/Best-README-Template)
- [Choose an Open Source License](https://choosealicense.com)
- [Img Shields](https://shields.io)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
[contributors-shield]: https://img.shields.io/github/contributors/LoveDoLove/CS_CommonUtilities.svg?style=for-the-badge
[contributors-url]: https://github.com/LoveDoLove/CS_CommonUtilities/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/LoveDoLove/CS_CommonUtilities.svg?style=for-the-badge
[forks-url]: https://github.com/LoveDoLove/CS_CommonUtilities/network/members
[stars-shield]: https://img.shields.io/github/stars/LoveDoLove/CS_CommonUtilities.svg?style=for-the-badge
[stars-url]: https://github.com/LoveDoLove/CS_CommonUtilities/stargazers
[issues-shield]: https://img.shields.io/github/issues/LoveDoLove/CS_CommonUtilities.svg?style=for-the-badge
[issues-url]: https://github.com/LoveDoLove/CS_CommonUtilities/issues
[license-shield]: https://img.shields.io/github/license/LoveDoLove/CS_CommonUtilities.svg?style=for-the-badge
[license-url]: https://github.com/LoveDoLove/CS_CommonUtilities/blob/master/LICENSE
[product-screenshot]: CommonUtilities/www/images/icon.png
