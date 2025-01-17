﻿/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Producing;

using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

internal static class Program
{
    private static async Task Main()
    {
        var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (sender, args) =>
        {
            cts.Cancel();
            args.Cancel = true;
        };

        await using var client = PulsarClient.Builder().Build(); // Connecting to pulsar://localhost:6650

        await using var producer = client.NewProducer(Schema.String)
            .StateChangedHandler(Monitor)
            .Topic("persistent://public/default/mytopic")
            .Create();

        Console.WriteLine("Press Ctrl+C to exit");

        await ProduceMessages(producer, cts.Token);
    }

    private static async Task ProduceMessages(IProducer<string> producer, CancellationToken cancellationToken)
    {
        var delay = TimeSpan.FromSeconds(5);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var data = DateTime.UtcNow.ToLongTimeString();
                _ = await producer.Send(data, cancellationToken);
                Console.WriteLine($"Sent: {data}");
                await Task.Delay(delay, cancellationToken);
            }
        }
        catch (OperationCanceledException) // If not using the cancellationToken, then just dispose the producer and catch ObjectDisposedException instead
        { }
    }

    private static void Monitor(ProducerStateChanged stateChanged, CancellationToken cancellationToken)
    {
        var topic = stateChanged.Producer.Topic;
        var state = stateChanged.ProducerState;
        Console.WriteLine($"The producer for topic '{topic}' changed state to '{state}'");
    }
}
