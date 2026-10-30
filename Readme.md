# TenForce C# Console Application - Extension

## Overview
This project extends the `Test-Taste-Console-Application` to fetch, calculate, and output planetary moon temperature data alongside existing system statistics from the Solar System OpenData API (`https://api.le-systeme-solaire.net/`).

## Summary of Changes

### 1. API & Data Transfer Layer (`UriPath.cs`, `MoonDto.cs`)
* **API Query Parameter:** Added `,avgTemp` to the celestial bodies query path to retrieve moon temperature properties from the API.
* **Data Transfer Object:** Extended `MoonDto` with the `AvgTemp` property (`[JsonProperty("avgTemp")`) to map incoming JSON temperature data correctly.

### 2. Domain Model (`Moon.cs`, `Planet.cs`)
* **`Moon` Entity:** Added the `AvgTemp` property (measured in Kelvin) to accurately represent physical celestial attributes.
* **`Planet` Aggregate Root:** Implemented the `AverageMoonTemperature` calculated property using LINQ (`Moons.Average(m => m.AvgTemp)`) to encapsulate temperature business logic directly within the entity.

### 3. Output Services & Disk Persistence (`ConsoleWriter.cs`)
* **Dual Output Pipeline:** Updated the `ConsoleWriter` utility to seamlessly mirror all formatted terminal output (headers, text rows, separators) to a file on disk simultaneously.
* **Timestamped & Organized Storage:** Automatically generates a unique timestamp-specific file (e.g., `SolarSystemOutput_20261030_142510.txt`) inside a dedicated `FileOutput` folder located in the project's root directory.
* **Robust Exception Handling & Logging:** Wrapped disk I/O operations in safe `try-catch` blocks integrated with `log4net` to log warnings and prevent unexpected crashes due to file locks or access permissions.
