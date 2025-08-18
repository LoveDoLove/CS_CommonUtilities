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

- **Security**: AES, SHA256, 3DES, signature, validation helpers
- **Data**: Conversion, formatting, JSON file operations
- **HTTP**: Basic and advanced HTTP utilities
- **System**: App settings, caching, file/process utilities, logging, timestamps
- **Scheduler**: Cron jobs, scheduled services, extensible SyncService base
- **Media**: Image processing helpers
- **QR Code**: QR code generation utilities
- **Stripe**: Payment integration helpers
- **Google MFA**: Multi-factor authentication helpers
- **Cloudflare Captcha**: Captcha validation helpers
- **Mailer**: SMTP and email sending helpers
- **IP Info**: IP geolocation and lookup helpers
- **Command**: Command-line helper utilities
- **Enum**: Enum parsing and conversion helpers
- **Database**: Strongly-typed models and DbContext
- **SyncService**: Base and example for scheduled background jobs
- **Configuration**: Easy integration with appsettings.json
- **Extensible**: Modular, production-ready, and open source

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [xUnit.net](https://xunit.net/) (for testing)
- [.NET Community Toolkit](https://github.com/CommunityToolkit/dotnet)

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

### Configuration

Many helpers require configuration via `appsettings.json` (or `appsettings.Development.json`). Example:

```json
{
  "Smtp": {
    "Host": "smtp.example.com",
    "Port": 587,
    "Username": "xxx@example.com",
    "Password": "example",
    "From": "no-reply@example.com",
    "Name": "example"
  },
  "CfCaptcha": {
    "SiteKey": "xxx",
    "SecretKey": "xxx"
  },
  "IpInfo": {
    "Token": "xxx"
  },
  "Stripe": {
    "ApiKey": "sk_test_xxx",
    "WebhookSecret": "whsec_xxx"
  }
}
```

Adjust these values for your environment. See the sample files for all options.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE EXAMPLES -->

## Usage

Import the relevant namespaces from `CommonUtilities` and use the helpers as needed. Example:

```csharp
using CommonUtilities.Utilities.Security;

string encrypted = AesUtilities.Encrypt("mydata", "password");
string hash = Sha256Utilities.ComputeHash("mydata");
```

### Advanced Usage

**MailerHelper with configuration:**

```csharp
using CommonUtilities.Helpers.Mailer;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();
var smtpConfig = config.GetSection("Smtp").Get<SmtpConfig>();
var mailer = new MailerHelper(smtpConfig);
await mailer.SendAsync("to@example.com", "Subject", "Body");
```

**StripeHelper:**

```csharp
using CommonUtilities.Helpers.Stripe;
var stripeConfig = new StripeConfig { ApiKey = "sk_test_xxx" };
var stripe = new StripeHelper(stripeConfig);
// Use stripe methods for payment, customer, etc.
```

**SyncService (scheduled job):**

```csharp
// Inherit from SyncServiceBase<T> and override ExecuteSyncAsync for your job logic.
```

For more usage examples, see the source code and XML documentation in each helper class.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP -->

## Roadmap

- [x] Modular helper structure
- [x] .NET 8.0 support
- [x] Security, data, HTTP, system, and scheduler utilities
- [x] Media, QR code, Stripe, Google MFA, Cloudflare Captcha, mailer, and IP info helpers
- [x] Command, Enum, Database, SyncService helpers
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
