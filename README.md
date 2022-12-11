# Shopy-Bot-V2-Discord-Payment-Bot-Send-Recevie-Ping-More

![LatestVersion](https://maszindex.zaanposni.com/api/v1/views/version/current/readme?)
[![https://github.com/users/zaanposni/packages/container/package/masz_backend](https://img.shields.io/badge/using-docker-blue?style=for-the-badge)](https://github.com/users/zaanposni/packages/container/package/masz_backend)
[![https://discord.gg/5zjpzw6h3S](https://img.shields.io/discord/779262870016884756?logo=discord&style=for-the-badge)](https://discord.gg/5zjpzw6h3S)
[![SupportedLanguages](https://img.shields.io/badge/translated-7%20languages-brightgreen?style=for-the-badge)](https://github.com/zaanposni/discord-masz/blob/master/translations/supported_languages.json) 

## Performant
- Thanks to utilizing [Erlang Text Format](https://github.com/RealTimeChris/DiscordCoreAPI/blob/main/Source/ErlPacker.cpp) for websocket transfer, and a pool of [kept-alive HTTP connections](https://github.com/RealTimeChris/DiscordCoreAPI/blob/main/Source/Http.cpp#L253) - this library offers the snappiest responses to your interactions and user input.

![Discord Bots](https://top.gg/api/widget/962093056910323743.svg)<br>

## Links

- **[Support Server](https://discord.gg/TsSAfdN4hS)**
- **[Invite Bot](https://discord.com/api/oauth2/authorize?client_id=962093056910323743&permissions=1374524140630&scope=bot%20applications.commands)**

## AI Spam Detection

The bot is able to detect spam messages and even sequences of messages that are spam at pretty high
accuracy. <!-- if well trained -->

### Quick Setup

1. Enable the system with `/antispam enable`
2. Set the ignored channels and roles with `/antispam ignore` command group

## NextGen Word Filter

AIAS also has an innovative word filter that can find blacklisted words in the most tough places, is able to filter out
bypass symbols, apply antialias and even **search curse words between messages!**

## CPU Efficient   
- It only uses about 0.1% of an Intel i7 9750h to stream audio in high quality (Opus 48Khz 16-bit Stereo) to a single server.   

## Entire Discord API Covered   
- All of the Discord API endpoints are covered in this library, including voice communication.   

## Concurrent Discord API Access   
- As a result of using [custom asynchronous coroutines](https://github.com/RealTimeChris/DiscordCoreAPI/blob/main/Include/discordcoreapi/CoRoutine.hpp) along with a [thread pool](https://github.com/RealTimeChris/DiscordCoreAPI/blob/main/Source/ThreadPool.cpp#L70), this library has the ability to make fully    asynchronous/concurrent requests to the Discord API.   

## Advanced Rate-Limiting System
- Guarantees that the order in which HTTP requests are executed is the same that they were submitted in - despite being launched across different threads, while never infracting on any of the Discord API's rate-limits and while running concurrently across all of the endpoints.


⭐ **Infractions and managed (temporary) punishments** - to moderate your server\
⭐ **Quicksearch** - to reliably search for any infractions or notes a user has\
⭐ **Localization** - timezones and languages are fully customizable\
⭐ **Automoderation** - to give trolls no chance\
⭐ **Ban appeals and webhook notifications** - to moderate your server transparently\
⭐ **A website and a discord bot** - to use MASZ\
⭐ **Full API and plugin support** - for custom scripts and automations

### 🚀 Demo

[![https://demomasz.zaanposni.com](https://img.shields.io/badge/demo-online-%234c1?style=for-the-badge)](https://demomasz.zaanposni.com)

Visit [https://demomasz.zaanposni.com](https://demomasz.zaanposni.com) for a demo.\
Furthermore, join the demo guild [https://discord.gg/7ubU6aWX9c](https://discord.gg/7ubU6aWX9c) to get the required permissions.

### 👀 Preview

![dashboard preview](/docs/dashboard.png)

**Previews and examples can be found at:** [https://github.com/zaanposni/discord-masz/tree/master/docs](https://github.com/zaanposni/discord-masz/tree/master/docs)

### 🤝 Support Server

Join this server to receive update information or get support: [https://discord.gg/5zjpzw6h3S](https://discord.gg/5zjpzw6h3S)

## 🛠 Hosting

You can **host your own instance of MASZ** by using the instructions below.\
If you have any questions, feel free to contact Zaanposni, or a support member:

- [Discord server](https://discord.gg/5zjpzw6h3S)
- [Email](mailto:me@zaanposni.com)

Note that MASZ is not hosted publicly. You will have to host your own instance.\
Furthermore, only deployments on a linux and windows server are supported. Read below for software requirements.\
Free hosting providers like replit or heroku **do not work**. You will have to use a VPS instead.

### TL;DR;

- Create a discord application at [https://discord.com/developers/applications](https://discord.com/developers/applications)
- Set redirect urls on your discord application [as defined](https://github.com/zaanposni/discord-masz#discord-oauth).
- Enable **Server Members** and **Message Content Intent** in your bot settings.
- Use `python3 setup.py` (`python setup.py` on windows) to setup the configuration.
- Start the application with `docker-compose up -d`.
- App will be hosted on `127.0.0.1:5565`.
- Read further for more information on different deployment methods and further steps.

### Discord OAuth

Create your own OAuth application [here](https://discord.com/developers/applications).
Also set the redirect paths in the tab `OAuth2`.\
Be sure to set the following (choose localhost or domain depending on your deployment):

![redirect example](/docs/redirects.png)
![redirect example 2](/docs/redirects2.png)

### Bot Intents

Enable **Server Members** and **Message Content Intent** in your bot settings.

![intents example](/docs/intents.png)

### Slash commands

If you have added your bot yourself, your bot might be missing the permission to create slashcommands.\
Use the following link to authorize your bot to do so `https://discord.com/api/oauth2/authorize?permissions=8&scope=bot%20applications.commands&client_id=yourid`.\
Be sure to replace "yourid" at the end with your client id.

## ✔️ Docker Setup (Recommended)

[![https://github.com/users/zaanposni/packages/container/package/masz_backend](https://img.shields.io/badge/using-docker-blue?style=for-the-badge)](https://github.com/users/zaanposni/packages/container/package/masz_backend)

### Requirements

- [docker](https://docs.docker.com/engine/install/ubuntu/) & [docker-compose](https://docs.docker.com/compose/) (`docker-compose -v` > 1.25)
- [python3](https://www.python.org/) for setup

#### If you want to deploy on a domain

- a (sub)domain to host the application on
- a reverse proxy on your host

### Instructions

- Download this repository `git clone https://github.com/zaanposni/discord-masz` ([zip link](https://codeload.github.com/zaanposni/discord-masz/zip/master))
- Use `python3 setup.py` (`python setup.py` on windows) to setup the configuration.
- Start the application with `docker-compose up -d`.
- App will be hosted on `127.0.0.1:5565`, if you are hosting the app on a domain, redirect your reverse proxy to this local port!

### Update

To install a new update of MASZ just use:

```bash
docker-compose pull
docker-compose up -d
```

## ↪ After Deployment

### 🐾 First Steps

- You can visit your application at `yourdomain.com` (or `127.0.0.1:5565`). You will see a login screen that will ask you to authenticate yourself using Discord OAuth2.
- After authorizing your service to use your Discord account you will see your profile picture in the toolbar (this is hosted on `127.0.0.1:8080` when developing).
- If you are logged in as a siteadmin, you can use the "register guild" (+) button to register your guilds and to get started.
- Based on wanted features and functionalities you might have to grant your bot advanced permissions, read under `Enabling Restricted Features`.

### Request logging and ratelimit

MASZ uses the `X-Forwarded-For` http header for logging and ratelimit.\
Ensure that this header is set in your reverse proxy for best experience.

### Enabling Restricted Features

#### ⭐ Unban request feature

If you want banned users to see their cases, grant your bot the `ban people` permission.\
This way they can see the reason for their ban and comment or send an unban request.\
Furthermore, make sure the bot is high enough in the role hierarchy to ban people below him.

Additionally, if you want to enable unban requests ("ban appeals"), prepare questions in the "appeals" section of your guild dashboard.

#### ⭐ Punishment feature

If you want the application to execute punishments like mutes and bans and manage them automatically (like unban after defined time on tempban), grant your bot the following permissions based on your needs:

```md
Manage roles - for muted role | Moderate members - for using discord's timeouts
Kick people
Ban people
```

Furthermore, make sure the bot is high enough in the role hierarchy to punish people below him.

If you do not want to use a role to mute members, MASZ will automatically use discord's timeouts if you dont define a muted role.

#### ⭐ Automoderation feature

To avoid any issue for message deletion or read permissions it is recommended to grant your bot a very high and strong or even the `administrator` role.

#### ⭐ Invite tracking

Allows MASZ to track the invites new members are using. Grant your bot the `manage guild` permission to use this feature.

#### ⭐ Strict permission check

You can enable strict permissions in your guildconfig. This mode will check your moderators role permissions before creating a modcase.\
A moderator can only create a kick or ban modcase if he has the respective permission in discord.\
If you do not enable this mode, moderators can create any modcase.
