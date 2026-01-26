# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [3.0.1] - 2025-01-26

### Fixed
- Fixed JavaScript interop property name casing (C# serializes as camelCase)
- Fixed utils.js path resolution for dynamic import (changed from relative to absolute path)
- Fixed validation returning null when utils script not yet loaded
- Fixed `InitialCountry="auto"` not working by adding geoIpLookup integration

### Changed
- Added support for all phone number types in validation (fixed-line, mobile, fixed-line-or-mobile)
- Improved async handling to wait for utils to load before validation

## [3.0.0] - 2025-01-26

### Added
- Multi-framework support for .NET 8, .NET 9, and .NET 10
- npm-based dependency management for intl-tel-input library
- `package.json` and `build-assets.js` for automated asset management
- MSBuild integration to automatically run npm install and copy assets during build
- Test for Belgian fixed line number validation

### Changed
- Upgraded intl-tel-input to v25.3.0
- Assets (JS, CSS, images) are now sourced from npm instead of manually vendored files
- Updated MudBlazor to v8.15.0

### Removed
- `data.js` - country data is now bundled internally in intl-tel-input v25
- Manually downloaded intl-tel-input files (now managed via npm)

### Dependencies
| Framework | Microsoft.AspNetCore.Components.Web | MudBlazor |
|-----------|-------------------------------------|-----------|
| net8.0    | 8.0.23                              | 8.15.0    |
| net9.0    | 9.0.12                              | 8.15.0    |
| net10.0   | 10.0.2                              | 8.15.0    |

### Breaking Changes
- Minimum supported framework is now .NET 8.0
- Requires Node.js/npm for building from source (assets are included in NuGet package)

## [1.x] - Previous Releases

See [GitHub Releases](https://github.com/JeremyVm/intl-tel-input-blazor/releases) for previous version history.
