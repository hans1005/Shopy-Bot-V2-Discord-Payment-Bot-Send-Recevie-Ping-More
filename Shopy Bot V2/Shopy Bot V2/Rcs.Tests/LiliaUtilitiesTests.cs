using System;
using Lilia.Commons;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lilia.Tests;

[TestClass]
public class LiliaUtilitiesTests
{
    private const string JumpLinkTestString = "https://discord.com/channels/708668574201544745/923884254298005525/940474669759365160";
    private const string DiscordBotInvTestString = "https://discord.com/oauth2/authorize?client_id=177013&scope=bot&permissions=8";
    private const string DiscordGuildInvTestString = "https://discord.gg/discord";

    [TestMethod]
    public void JumpLinkResolveGuildTest()
    {
        Tuple<ulong, ulong, ulong> result = JumpLinkTestString.ResolveDiscordMessageJumpLink();

        ulong actualGuildId = 708668574201544745;
        ulong expectedGuildId = result.Item1;
        
        Assert.AreEqual(actualGuildId, expectedGuildId);
    }
    
    [TestMethod]
    public void JumpLinkResolveChannelTest()
    {
        Tuple<ulong, ulong, ulong> result = JumpLinkTestString.ResolveDiscordMessageJumpLink();

        ulong actualChannelId = 923884254298005525;
        ulong expectedChannelId = result.Item2;
        
        Assert.AreEqual(actualChannelId, expectedChannelId);
    }
    
    [TestMethod]
    public void JumpLinkResolveMessageTest()
    {
        Tuple<ulong, ulong, ulong> result = JumpLinkTestString.ResolveDiscordMessageJumpLink();

        ulong actualMsgId = 940474669759365160;
        ulong expectedMsgId = result.Item3;
        
        Assert.AreEqual(actualMsgId, expectedMsgId);
    }

    [TestMethod]
    public void IsValidDiscordBotInviteSuccessTest()
    {
        bool res = DiscordBotInvTestString.IsDiscordValidBotInvite();
        Assert.AreEqual(res, true);
    }

    [TestMethod]
    public void IsValidDiscordBotInviteFailEmptyTest()
    {
        bool res = string.Empty.IsDiscordValidBotInvite();
        Assert.AreEqual(res, false);
    }
    
    [TestMethod]
    public void IsValidDiscordBotInviteFailNullTest()
    {
        string test = null;
        bool res = test.IsDiscordValidBotInvite();
        Assert.AreEqual(res, false);
    }

    [TestMethod]
    public void IsValidDiscordBotInviteFailInvalidTest()
    {
        bool res = "https://rickroll.discord.com/".IsDiscordValidBotInvite();
        Assert.AreEqual(res, false);
    }
    
    [TestMethod]
    public void IsValidDiscordGuildInviteSuccessTest()
    {
        bool res = DiscordGuildInvTestString.IsDiscordValidGuildInvite();
        Assert.AreEqual(res, true);
    }

    [TestMethod]
    public void IsValidDiscordGuildInviteFailEmptyTest()
    {
        bool res = string.Empty.IsDiscordValidGuildInvite();
        Assert.AreEqual(res, false);
    }
    
    [TestMethod]
    public void IsValidDiscordGuildInviteFailNullTest()
    {
        string test = null;
        bool res = test.IsDiscordValidGuildInvite();
        Assert.AreEqual(res, false);
    }

    [TestMethod]
    public void IsValidDiscordGuildInviteFailInvalidTest()
    {
        bool res = "https://rolled.gg/discord".IsDiscordValidGuildInvite();
        Assert.AreEqual(res, false);
    }
}