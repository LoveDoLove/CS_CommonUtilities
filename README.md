<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->
<a id="readme-top"></a>

<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <img src="images/icon.png" alt="Logo" width="80" height="80">
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

CommonUtilities is a modular, production-ready C#/.NET utility library and toolkit designed to accelerate development for .NET 8+ projects. It provides a comprehensive set of helpers, utilities, and models for common tasks such as security, data formatting, HTTP, scheduling, mailing, image processing, and more. The library is structured for easy integration and extension, making it ideal for both small and large-scale applications.

### Features
- Modular helpers for Captcha, Google MFA, IP info, mailing, media, scheduling, Stripe, and more
- Utility classes for data conversion, formatting, JSON, database, HTTP, enums, QR codes, security, system operations, and logging
- Strong focus on security, input validation, and error handling
- Designed for .NET 8.0 and above
- MIT licensed and open for contributions

### Built With
- .NET 8.0
- [Cronos](https://github.com/HangfireIO/Cronos)
- [FluentValidation](https://fluentvalidation.net/)
- [GoogleAuthenticator](https://github.com/brandonpotter/GoogleAuthenticator)
- [IPinfo](https://github.com/ipinfo/csharp)
- [MailKit](https://github.com/jstedfast/MailKit)
- [MediatR](https://github.com/jbogard/MediatR)
- [Serilog](https://serilog.net/)
- [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp)
- [Stripe.net](https://github.com/stripe/stripe-dotnet)
- [QRCoder](https://github.com/codebude/QRCoder)
- [RestSharp](https://github.com/restsharp/RestSharp)
- [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [Newtonsoft.Json](https://www.newtonsoft.com/json)

<!-- GETTING STARTED -->
## Getting Started

To use CommonUtilities in your .NET project, follow these steps:

### Prerequisites
- .NET 8.0 SDK or later

### Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/LoveDoLove/CS_CommonUtilities.git
   ```
2. Add a reference to the `CommonUtilities` project or build and reference the generated DLL in your solution.
3. (Optional) Configure any required settings (e.g., for mailing, Stripe, etc.) in your appsettings or via dependency injection.

<!-- USAGE EXAMPLES -->
## Usage

Below are some example usages. For more, see the source code and documentation.

### Example: Sending an Email
```csharp
var mailer = new MailerHelper(new MailerConfig { /* ... */ });
await mailer.SendAsync("to@example.com", "Subject", "Body");
```

### Example: Validating a Captcha
```csharp
var captcha = new CfCaptchaHelper(new CfCaptchaConfig { /* ... */ });
bool isValid = await captcha.ValidateAsync("user-response");
```

### Example: Generating a QR Code
```csharp
var qr = QrCodeUtilities.Generate("Hello World");
```

_For more examples, please refer to the [Documentation](https://github.com/LoveDoLove/CS_CommonUtilities)_

<!-- ROADMAP -->
## Roadmap
- [x] Modular helpers for common services
- [x] Security and validation utilities
- [x] Logging and diagnostics
- [ ] Add more integration samples
- [ ] Expand documentation and usage guides

See the [open issues](https://github.com/LoveDoLove/CS_CommonUtilities/issues) for a full list of proposed features (and known issues).

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<!-- CONTACT -->
## Contact

LoveDoLove - [@LoveDoLove](https://github.com/LoveDoLove)

Project Link: [https://github.com/LoveDoLove/CS_CommonUtilities](https://github.com/LoveDoLove/CS_CommonUtilities)

<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

- [Best-README-Template](https://github.com/othneildrew/Best-README-Template)
- [Choose an Open Source License](https://choosealicense.com)
- [GitHub Emoji Cheat Sheet](https://www.webpagefx.com/tools/emoji-cheat-sheet)
- [Img Shields](https://shields.io)
- [Font Awesome](https://fontawesome.com)

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
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/LoveDoLove
