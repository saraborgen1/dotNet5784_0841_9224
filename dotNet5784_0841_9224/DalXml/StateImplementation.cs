﻿using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal class StateImplementation:IDal
    {
        public DateTime? StartProject
        {
            get
            {
                XElement root = LoadListFromXMLElement("data-config");
                return DateTime.TryParse((string?)root.Element("StartProject"), out var result) ? (DateTime?)result : null;
            }
            set
            {
                XElement root = LoadListFromXMLElement("data-config");
                root.Element("StartProject")?.SetValue(value!);
                SaveListToXMLElement(root, "data-config");
            }

        }


        public DateTime? EndProject
        {
            get
            {
                XElement root = XElement.Load(@"..\xml\data_config.xml");
                return DateTime.TryParse((string?)root.Element("EndProject"), out var result) ? (DateTime?)result : null;

            }
            set
            {
                XElement root = XElement.Load(@"..\xml\data_config.xml"); ;
                root.Element("EndProject")?.SetValue(value?.ToString());
                root.Save(@"..\xml\data_config.xml");
            }
        }

        public ITask Task => throw new NotImplementedException();

        public IDependency Dependency => throw new NotImplementedException();

        public IEngineer Engineer => throw new NotImplementedException();

        public ProjectStatus StatusProject()
        {
            if (StartProject == null)
                return ProjectStatus.Creation;
            if (Task.Read(p => p.StartDate == null) != null)
                return ProjectStatus.Scheduling;
            return ProjectStatus.Start;
        }
    }
}