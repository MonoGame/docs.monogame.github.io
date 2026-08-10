protected void ConnectToHost()
{
    Debug.WriteLine($"{DateTime.UtcNow:s}::User {_user.Id} is connecting to host...");

    var connectionResult = _networkService.ConnectToHost(_user);
    if (connectionResult.State == ConnectionState.Success)
    {
        Debug.WriteLine($"{DateTime.UtcNow:s}::User connected to host {connectionResult.Host}.");
    }
    else
    {
        Debug.WriteLine($"{DateTime.UtcNow:s}::User failed to connect to host {connectionResult.Host}.\nConnection state: {connectionResult.State}\nState: {connectionResult.State}, Exception: {connectionResult.Exception}");
    }
}