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

namespace DotPulsar.Internal.Events
{
    using Abstractions;
    using System;

    /// <summary>
    /// Representation of the sub producer of a partitioned producer state change.
    /// </summary>
    public sealed class PartitionedSubProducerStateChanged : IEvent
    {
        public Guid CorrelationId { get; }
        public ProducerState ProducerState { get; }

        public PartitionedSubProducerStateChanged(Guid correlationId, ProducerState producerState)
        {
            CorrelationId = correlationId;
            ProducerState = producerState;
        }
    }
}