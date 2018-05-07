using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    class Initializator
    {
        private BuildingInitializator buildingInitializator;
        private ManagerInitializator managerInitializator;
        private PassengerCreator passengerCreator;

        public Initializator(BuildingInitializator buildingInitializator, ManagerInitializator managerInitializator,
            PassengerCreator passengerCreator)
        {
            this.buildingInitializator = buildingInitializator;
            this.managerInitializator = managerInitializator;
            this.passengerCreator = passengerCreator;
        }

        public void Initialize()
        {
            buildingInitializator.InitializeBuilding(3, 4);
            managerInitializator.InitializeDispatcher();
            managerInitializator.InitializeManagers();
            managerInitializator.SubscribeOnEvents();
            passengerCreator.StartPassengerGenerator();
        }
    }
}
