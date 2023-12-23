// -----------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System.Windows;
using Chapter.Net.Networking.Broadcast;

namespace DemoBroadcastServer;

public partial class MainWindow
{
    private IBroadcasting _broadcasting;
    private ServerToken _token;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnStartServerClicked(object sender, RoutedEventArgs e)
    {
        OnStopServerClicked(sender, e);

        messages.Items.Add("Start server");
        _broadcasting = new Broadcasting();
        _token = _broadcasting.Start(new ServerConfiguration(37455, "Hello Client", s => s == "Hello Server"));
        _broadcasting.ClientMessageReceiving += OnClientMessageReceiving;
        _broadcasting.ClientMessageReceived += OnClientMessageReceived;
    }

    private void OnClientMessageReceiving(object sender, ClientMessageReceivingEventArgs e)
    {
        messages.Dispatcher.Invoke(() => messages.Items.Add($"'{e.Address}' has send '{e.Message}' and we will reply '{e.Configuration.ResponseMessage}'"));
    }

    private void OnClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
    {
        messages.Dispatcher.Invoke(() => messages.Items.Add($"'{e.Address}' has send '{e.Message}' and we replied with '{e.Configuration.ResponseMessage}'"));
    }

    private void OnStopServerClicked(object sender, RoutedEventArgs e)
    {
        if (_broadcasting != null)
        {
            messages.Items.Add("Stop server");
            _broadcasting.Dispose(_token);
            _broadcasting = null;
        }
    }
}