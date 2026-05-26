namespace Kron.Counting.Shared.Settings;

using System;
using System.Collections.Generic;
using System.Text;
public sealed class RedisSettings
{
    public string ConnectionString { get; set; } = string.Empty;

    public string InstanceName { get; set; } = string.Empty;
}