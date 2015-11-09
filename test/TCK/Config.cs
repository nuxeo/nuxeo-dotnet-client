/*
 * (C) Copyright 2015 Nuxeo SA (http://nuxeo.com/) and others.
 *
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the GNU Lesser General Public License
 * (LGPL) version 2.1 which accompanies this distribution, and is available at
 * http://www.gnu.org/licenses/lgpl-2.1.html
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * Contributors:
 *     Gabriel Barata <gbarata@nuxeo.com>
 */

using Microsoft.Framework.Configuration;
using System;
using System.IO;

namespace TCK
{
    public class Config
    {
        private IConfiguration config;

        private static readonly Lazy<Config> instance = new Lazy<Config>(() => new Config());

        private Config()
        {
            if (File.Exists("config.json"))
                config = new ConfigurationBuilder().AddJsonFile("config.json").Build();
            else
                config = null;
        }

        public static Config Instance { get { return instance.Value; } }

        public string GetServerUrl()
        {
            if (config != null && config.GetSection("ServerURL") != null)
            {
                return (string)config.GetSection("ServerURL").Value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}