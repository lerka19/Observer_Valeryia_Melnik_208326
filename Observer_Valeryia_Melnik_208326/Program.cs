using System;
using System.Collections.Generic;

namespace Observer_Valeryia_Melnik_208326
{
    public interface ParkingObserver
    {
        void Update(ISubject subject);
    }
    public interface ISubject
    {
        void Attach(ParkingObserver observer);
        void Detach(ParkingObserver observer);
        void Notify();
    }
    public class Parking : ISubject
    {
        public int freeSpots;
        private List<ParkingObserver> observers;
        private Dictionary<string, DateTime> number;
        public double seconds;

        public Parking()
        {
            this.freeSpots = 100;
            this.observers = new List<ParkingObserver>();
            this.number = new Dictionary<string, DateTime>();
        }
        public void Attach(ParkingObserver observer)
        {
            observers.Add(observer);
        }
        public void Detach(ParkingObserver observer)
        {
            observers.Remove(observer);
        }
        public void IncreaseFreeSpots(string registrationNumber)
        {
            if (number.Count == 0)
            {
                Console.WriteLine("To nie jest możliwe! Na parkingu nie ma samochodów!");
            }
            else if (number.ContainsKey(registrationNumber))
            {
                freeSpots++;
                DateTime lastTime = number[registrationNumber];
                TimeSpan duration = DateTime.Now - lastTime;
                number.Remove(registrationNumber);
                seconds = duration.TotalSeconds;
                Notify();
            }
            else
            {
                Console.WriteLine("Błąd! Brak samochodu o takim numerze!");
            }
        }
        public void DecreaseFreeSpots(string registrationNumber)
        {
            if (number.ContainsKey(registrationNumber))
            {
                Console.WriteLine("Sprawdź dane jeszcze raz! Samochód o takim numerze już stoi na parkingu!");
            }
            else
            {
                freeSpots--;
                number.Add(registrationNumber, DateTime.Now);
                Notify();
            }
        }
        public void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update(this);
            }
        }
    }
    class ParkingDisplay : ParkingObserver
    {
        public void Update(ISubject subject)
        {
            Console.WriteLine("Liczba wolnych miejsc postojowych: " + (subject as Parking).freeSpots);
        }
    }
    class ParkingReceipt : ParkingObserver
    {
        public void Update(ISubject subject)
        {
            double price = (subject as Parking).seconds * 0.1;
            Console.WriteLine("Do drukarki wysłano paragon o wartości: " + price + " PLN");
        }
    }
    class ParkingController
    {
        private Parking parking;

        public ParkingController(Parking parking)
        {
            this.parking = parking;
        }
        public void HandleInput(string operation, string registrationNumber)
        {
            Console.WriteLine("Podany rodzaj operacji(in/out): " + operation);
            Console.WriteLine("Podany numer rejestracyjny samochodu: " + registrationNumber);
            if (operation == "in")
            {
                var display = new ParkingDisplay();
                parking.Attach(display);
                parking.DecreaseFreeSpots(registrationNumber);
                parking.Detach(display);
            }
            else if (operation == "out")
            {
                var display = new ParkingDisplay();
                var receipt = new ParkingReceipt();
                parking.Attach(display);
                parking.Attach(receipt);
                parking.IncreaseFreeSpots(registrationNumber);
                parking.Detach(display);
                parking.Detach(receipt);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var parking = new Parking();
            var controller = new ParkingController(parking);
            controller.HandleInput("out", "AW4324");
            controller.HandleInput("in", "AW4324");
            controller.HandleInput("in", "WE4567");
            controller.HandleInput("in", "OI9745");
            controller.HandleInput("in", "PL2309");
            controller.HandleInput("in", "SA1008");
            controller.HandleInput("in", "AA0000");
            controller.HandleInput("in", "RT5610");
            controller.HandleInput("out", "AW4324");
            controller.HandleInput("out", "WE4567");
            controller.HandleInput("in", "OI9745");
            controller.HandleInput("in", "GH9345");
            controller.HandleInput("out", "WE4587");
        }
    }
}
