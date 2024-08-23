// -----------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using Chapter.Net.Networking.Broadcast;

namespace DemoBroadcastClient;

public partial class MainWindow
{
    private readonly IBroadcasting _broadcasting;

    public MainWindow()
    {
        InitializeComponent();

        _broadcasting = new Broadcasting();
    }

    private void OnSendMessageClicked(object sender, RoutedEventArgs e)
    {
        var configuration = new ClientConfiguration(37455, "Hello Server", TimeSpan.FromSeconds(10));
        _broadcasting.Send(configuration, response => { messages.Dispatcher.Invoke(() => messages.Items.Add($"'{response.Address}' as reply to the '{response.Configuration.Message}' with '{response.Message}'")); });
    }
}