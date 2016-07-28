﻿/*******************************************************************************
 * Copyright (c) 2015-2016 Apcera Inc. All rights reserved. This program and the accompanying
 * materials are made available under the terms of the MIT License (MIT) which accompanies this
 * distribution, and is available at http://opensource.org/licenses/MIT
 *******************************************************************************/
using System;

namespace STAN.Client
{
    /// <summary>
    /// A received NATS streaming message.
    /// </summary>
    public class StanMsg
    {
        internal MsgProto proto;
        private  AsyncSubscription sub;

        internal StanMsg(MsgProto p, AsyncSubscription s)
        {
            this.proto = p;
            this.sub = s;
        }

        /// <summary>
        /// Get the time stamp of the message represeneted as Unix nanotime.
        /// </summary>
        public long Time
        {
            get
            {
                return proto.Timestamp;
            }
        }

        /// <summary>
        /// Get the timestamp of the message.
        /// </summary>
        public DateTime TimeStamp
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddTicks(proto.Timestamp/100);
            }
        }

        /// <summary>
        /// Acknowledge a message.
        /// </summary>
        public void Ack()
        {

            if (sub == null)
            {
                throw new StanBadSubscriptionException();
            }

            sub.manualAck(this);
        }

        public ulong Sequence
        {
            get
            {
                return proto.Sequence;
            }
        }

        public string Subject
        {
            get
            {
                return proto.Subject;
            }
        }

        public byte[] Data
        {
            get
            {
                if (proto.Data == null)
                    return null;

                return proto.Data.ToByteArray();
            }
        }

        /// <summary>
        /// The redelivered property if true if this message has been redelivered, false otherwise.
        /// </summary>
        public bool Redelivered
        {
            get { return proto.Redelivered; }
        }

        /// <summary>
        /// Gets the subscription this message was received upon.
        /// </summary>
        public IStanSubscription Subscription
        {
            get { return sub; }
        }
    }
}
