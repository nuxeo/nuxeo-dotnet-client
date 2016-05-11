/*
 * (C) Copyright 2016 Nuxeo SA (http://nuxeo.com/) and others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Contributors:
 *     Gabriel Barata <gbarata@nuxeo.com>
 */

using System.IO;
using System.Threading.Tasks;

namespace NuxeoClient
{
    /// <summary>
    /// Provides helper methods for IO operations.
    /// </summary>
    public class IOHelper
    {
        /// <summary>
        /// Creates a temporary file.
        /// </summary>
        /// <returns>Returns the file's <see cref="FileInfo"/>.</returns>
        public static FileInfo CreateTempFile()
        {
            string filePath = Path.GetTempFileName();
            FileInfo fileInfo = new FileInfo(filePath);
            fileInfo.Attributes = FileAttributes.Temporary;
            return fileInfo;
        }

        /// <summary>
        /// Creates a temporary file with the specified string as a content.
        /// </summary>
        /// <param name="content">The file content.</param>
        /// <returns>Returns the file's <see cref="FileInfo"/>.</returns>
        public static FileInfo CreateTempFile(string content)
        {
            FileInfo file = CreateTempFile();
            using (StreamWriter writer = file.AppendText())
            {
                writer.Write(content);
            }
            file.Refresh();
            return file;
        }

        /// <summary>
        /// Creates a temporary file from a supplied <see cref="Stream"/>.
        /// </summary>
        /// <param name="content">The stream from which to create a file.</param>
        /// <returns>Returns the file's <see cref="FileInfo"/>.</returns>
        public static FileInfo CreateTempFile(Stream content)
        {
            FileInfo file = CreateTempFile();
            using (FileStream fileStream = file.OpenWrite())
            {
                content.Seek(0, SeekOrigin.Begin);
                content.CopyTo(fileStream);
            }
            file.Refresh();
            return file;
        }

        /// <summary>
        /// Reads a file as a text file.
        /// </summary>
        /// <param name="path">The file's path.</param>
        /// <returns>The file's content.</returns>
        public static string ReadText(string path)
        {
            using (StreamReader reader = File.OpenText(path))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Reads a file as a text file.
        /// </summary>
        /// <param name="file">The file's <see cref="FileInfo"/>.</param>
        /// <returns>The file's content.</returns>
        public static string ReadText(FileInfo file)
        {
            using(StreamReader reader = file.OpenText())
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Reads a file as a text file asynchronously.
        /// </summary>
        /// <param name="path">The file's path.</param>
        /// <returns>The file's content.</returns>
        public async static Task<string> ReadTextAsync(string path)
        {
            using (StreamReader reader = File.OpenText(path))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Reads a file as a text file asynchronously.
        /// </summary>
        /// <param name="file">The file's <see cref="FileInfo"/>.</param>
        /// <returns>The file's content.</returns>
        public async static Task<string> ReadTextAsync(FileInfo file)
        {
            using (StreamReader reader = file.OpenText())
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Compares two files based on their contents.
        /// Source: https://support.microsoft.com/en-us/kb/320348
        /// </summary>
        /// <param name="file1">One file to be compared.</param>
        /// <param name="file2">Another file to be compared.</param>
        /// <returns><c>true</c> if both files' contents are equal; false otherwise.</returns>
        public static bool AreFilesEqual(string file1, string file2)
        {
            int file1byte;
            int file2byte;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            using (FileStream fs1 = File.OpenRead(file1))
            {
                using (FileStream fs2 = File.OpenRead(file2))
                {

                    // Check the file sizes. If they are not the same, the files 
                    // are not the same.
                    if (fs1.Length != fs2.Length)
                    {
                        // Return false to indicate files are different
                        return false;
                    }

                    // Read and compare a byte from each file until either a
                    // non-matching set of bytes is found or until the end of
                    // file1 is reached.
                    do
                    {
                        // Read one byte from each file.
                        file1byte = fs1.ReadByte();
                        file2byte = fs2.ReadByte();
                    }
                    while ((file1byte == file2byte) && (file1byte != -1));
                    
                    // Return the success of the comparison. "file1byte" is 
                    // equal to "file2byte" at this point only if the files are 
                    // the same.
                    return ((file1byte - file2byte) == 0);
                }
            }
        }
    }
}