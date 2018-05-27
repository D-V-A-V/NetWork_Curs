using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bridge
{
    class RecordMAC
    {
        public string MAC;
        public int timeToUpdate;

        public RecordMAC(int timeToCreate, string mac)
        {
            timeToUpdate = timeToCreate;
            MAC = mac;
        }

        public void UpdateIP(int Updatetime)
        {
            timeToUpdate = Updatetime;
        }

    }
}