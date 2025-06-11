# Carbon QuickBooks Desktop Sync

![Carbon QuickBooks Sync](https://github.com/user-attachments/assets/f4d26c1d-6844-4de7-b767-84819967c661)


This is a WinForms app that syncs data between Carbon and QuickBooks Desktop

- Contacts are synced from QuickBooks to Carbon
- Shipments and Purchase Orders are synced from Quickbooks to Carbon

It uses a 64-bit configuration

### Installing the package

- Clone the repo and run the `setup.exe` in `bin/publish` to install the Application

### Running the application
1. With the application installed and opened, navigate to the Settings tab
2. The API URL and public key should be pre-populated, but you can get them from the API Docs page if needed
3. In the Carbon application, navigate to Settings > API Keys and generate an API key, and save this to the settings.
4. In QuickBooks, get the account numbers for purchases and sales revenue, and enter them in the settings.
5. Save the settings, and you're ready to sync.

### Local Development

```
dotnet run
```

### Publishing the Project

In Visual Studio 2022, select Build > Publish Selection. The ClickOnce profile should be pre-selected. Clicking the Publish button should update the `bin/publish` folder.
