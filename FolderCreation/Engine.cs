using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Linq;
using System.Collections.Generic;

namespace FolderCreation
{
    public static class Engine
    {
        public static void Run()
        {
            Console.WriteLine($"Please note this only works on Windows.");
            Console.WriteLine("Please type in the full path to be created: (i.e. 'C:\\FolderName\\')");
            var directoryPath = Console.ReadLine();

            var rightFound = GetFileSystemRights();

            var controlTypeFound = GetAccessControlType();

            var domainUsername = GetDomainUsername();

            var directory = Directory.CreateDirectory(directoryPath);
            var security = directory.GetAccessControl();

            security.AddAccessRule(new FileSystemAccessRule(domainUsername, rightFound, controlTypeFound));

            directory.SetAccessControl(security);
        }

        private static FileSystemRights GetFileSystemRights()
        {
            var messages = new List<string>()
            {
                "\nPlease type the File System Right: "
            };
            messages.AddRange(Enum.GetValues(typeof(FileSystemRights)).Cast<FileSystemRights>().Select(x => x.ToString()));
            messages.Add("************************************");

            var chosenRight = GetUserInput(messages);

            FileSystemRights rightFound;
            var parsedRights = true;

            do
            {
                if (!parsedRights)
                    chosenRight = GetUserInput($"Please type the File System Right as shown above in the list: ");

                parsedRights = Enum.TryParse(chosenRight, out rightFound);
            } while (!parsedRights);

            return rightFound;
        }

        private static AccessControlType GetAccessControlType()
        {
            var messages = new List<string>()
            {
                "\nPlease type the Access Control Type: "
            };
            messages.AddRange(Enum.GetValues(typeof(AccessControlType)).Cast<AccessControlType>().Select(x => x.ToString()));
            messages.Add("************************************");

            var chosenControl = GetUserInput(messages);

            AccessControlType controlTypeFound;
            var parsedControlType = true;

            do
            {
                if (!parsedControlType)
                    chosenControl = GetUserInput($"Please type the File System Right as shown above in the list: ");

                parsedControlType = Enum.TryParse(chosenControl, out controlTypeFound);
            } while (!parsedControlType);

            return controlTypeFound;
        }

        private static string GetDomainUsername()
        {
            var domainUsername = $"{Environment.UserDomainName}\\{Environment.UserName}";

            var answer = GetUserInput("\nWould you like to type your own domain\\username? Type 'Y' for yes.");

            if (answer.ToLower() == "y")
                domainUsername = GetUserInput("\nPlease type the domain\\username you would like to use. Note: The '\\' in between is important.");

            return domainUsername;
        }

        private static string GetUserInput(IEnumerable<string> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            string userInput;

            do
            {
                messages.ToList().ForEach(x => Console.WriteLine(x));
                userInput = Console.ReadLine();

            } while (string.IsNullOrEmpty(userInput));

            return userInput;
        }

        private static string GetUserInput(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            string userInput;

            do
            {
                Console.WriteLine(message);
                userInput = Console.ReadLine();

            } while (string.IsNullOrEmpty(userInput));

            return userInput;
        }

    }
}
