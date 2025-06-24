<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->
<a id="readme-top"></a>

<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/LoveDoLove/CS_CommonUtilities">
    <img src="images/icon.png" alt="Logo" width="80" height="80">
  </a>
  <h3 align="center">CommonUtilities</h3>
  <p align="center">
    A modular, production-ready C#/.NET utility library and toolkit for rapid development.
    <br />
    <a href="https://github.com/LoveDoLove/CS_CommonUtilities"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/LoveDoLove/CS_CommonUtilities">View Demo</a>
    &middot;
    <a href="https://github.com/LoveDoLove/CS_CommonUtilities/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    &middot;
    <a href="https://github.com/LoveDoLove/CS_CommonUtilities/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
  </p>
</div>

<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#features">Features</a></li>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li><a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

CommonUtilities is a modular, production-ready C#/.NET utility library and toolkit designed to accelerate development for .NET 8+ projects. It provides a comprehensive set of helpers, models, and utilities for common application needs, including security, data, HTTP, scheduling, media, and more. The project is structured for easy extension and integration into any .NET solution.

### Features
- Modular helpers for:
  - Security (AES, SHA256, 3DES, signature, validation)
  - Data (conversion, formatting, JSON file operations)
  - HTTP (basic and advanced utilities)
  - System (app settings, caching, file/process utilities, logging, timestamps)
  - Scheduler (cron jobs, scheduled services)
  - Media (image processing)
  - QR code generation
  - Stripe payment integration
  - Google MFA and Cloudflare Captcha
  - Email (SMTP, mailer)
  - IP info lookup
- Strongly-typed models for database and shared data
- Designed for .NET 8.0 and above
- MIT licensed, open source

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With
- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) ([Context7 documentation used](https://github.com/dotnet/docs))
- [xUnit.net](https://xunit.net/) ([Context7 documentation used](https://xunit.net/docs/getting-started/netcore/cmdline))
- [.NET Community Toolkit](https://github.com/CommunityToolkit/dotnet) ([Context7 documentation used](https://aka.ms/mvvmtoolkit))

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

To use CommonUtilities in your .NET project, follow these steps:

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- (Optional) [xUnit.net](https://xunit.net/) for testing

### Installation
1. Clone the repository:
   ```cmd
   git clone https://github.com/LoveDoLove/CS_CommonUtilities.git
   ```
2. Add the project or reference the compiled DLL in your solution.
3. (Optional) Restore and run tests:
   ```cmd
   dotnet restore
   dotnet test
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE EXAMPLES -->
## Usage

Import the relevant namespaces from `CommonUtilities` and use the helpers as needed. Example:

```csharp
using CommonUtilities.Utilities.Security;

string encrypted = AesUtilities.Encrypt("mydata", "password");
string hash = Sha256Utilities.ComputeHash("mydata");
```

For more usage examples, see the source code and XML documentation in each helper class.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP -->
## Roadmap
- [x] Modular helper structure
- [x] .NET 8.0 support
- [x] Security, data, HTTP, and system utilities
- [x] Scheduler, media, QR code, Stripe, Google MFA, Cloudflare Captcha, mailer, and IP info helpers
- [ ] Add more advanced examples and documentation
- [ ] Expand test coverage
- [ ] Add more integration samples

See the [open issues](https://github.com/LoveDoLove/CS_CommonUtilities/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

To contribute:
1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

Please follow the [Context7](https://context7.com/) and .NET Foundation code of conduct and best practices.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTACT -->
## Contact

LoveDoLove - [GitHub](https://github.com/LoveDoLove)

Project Link: [https://github.com/LoveDoLove/CS_CommonUtilities](https://github.com/LoveDoLove/CS_CommonUtilities)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

- [Best-README-Template](https://github.com/othneildrew/Best-README-Template)
- [Choose an Open Source License](https://choosealicense.com)
- [.NET Foundation](https://dotnetfoundation.org/)
- [Context7 Documentation](https://context7.com/)
- [xUnit.net](https://xunit.net/)
- [CommunityToolkit](https://github.com/CommunityToolkit/dotnet)

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
[license-url]: https://github.com/LoveDoLove/CS_CommonUtilities/blob/main/LICENSE
[product-screenshot]: images/icon.png
