﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CUI = ConsoleUI.ConsoleUI;

namespace StoreManager.UserInterface.ApplicationInterface
{
    /// <summary>
    /// Represents a verbose interface that interacts with the StoreManager singleton
    /// </summary>
    public class VerboseApplicationInterface : ApplicationInterfaceBase
    {
        private readonly Dictionary<string, Action> _actions;

        public VerboseApplicationInterface() {
            _actions = new Dictionary<string, Action>
            {
            };
        }

        private void ManageCustomers() {

        }

        private void ManageStores() {

        }

        private void ManageProducts() {

        }

        private void ManageOrders() {

        }

        public override void Run() {

        }
    }
}