       void aa()
        {
            NetworkInterface[] NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface NetworkIntf in NetworkInterfaces)
            {
                IPInterfaceProperties IPInterfaceProperties = NetworkIntf.GetIPProperties();
                UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                {
                    if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        comboBox1.Items.Add(UnicastIPAddressInformation.Address);
                    }
                }
            }
            comboBox1.SelectedIndex = 0;
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.Items[i].ToString().IndexOf("192.") == 0)
                    comboBox1.SelectedIndex = i;
            }
        }