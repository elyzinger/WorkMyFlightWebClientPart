using WorkMyFlight.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight 
{
    public class AnonymousUserFacade : FacadeBase, IAnonymousUserFacade, IFacade
    {
        //  get a list of all the airline companies
        public IList<AirLineCompany> GetAllAirlineCompanies()
        {
            return _airlineDAO.GetAll();
        }
        //  get a list of all the flights
        public IList<Flight> GetAllFlights()
        {
            return _flightDAO.GetAll();
        }
        //  get vanancy of all flights with
        public Dictionary<Flight, long> GetAllFlightsVacancy()
        {
            return _flightDAO.GetAllFlightsVacancy();
        }
        //  get a flight by id
        public Flight GetFlightById(long id)
        {
            return _flightDAO.GetFlightById(id);
        }
        //  get a flight by DepartrureDate
        public IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate)
        {
            return _flightDAO.GetFlightByDepartureDate(departureDate);
        }
        //  get a flight by DestinationCountry
        public IList<Flight> GetFlightsByDestinationCountry(long countryCode)
        {
            return _flightDAO.GetFlightByDestinationCountry(countryCode);
        }
        //  get a flight by LandingDate
        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            return _flightDAO.GetFlightByLandingDate(landingDate);
        }
        //  get a flight by OriginCountry
        public IList<Flight> GetFlightsByOriginCountry(long countryCode)
        {
            return _flightDAO.GetFlightByOriginCountry(countryCode);
        }
        public IList<Flight> GetDepartingNow()
        {
            return _flightDAO.GetDepartures();
        }
        public IList<Flight> GetArrivingNow()
        {
            return _flightDAO.GetArrivels();
        }
        public IList<Flight> GetFlightsBySearch(SearchParam search)
        {
            IList<Flight> searchlist = new List<Flight>();
            if(search != null)
            {
                if (!string.IsNullOrEmpty(search.AirlineName))
                {
                    return  _flightDAO.GetAllFlightByAirlineName(search.AirlineName, search.FlightType);
                }
                if (!string.IsNullOrEmpty(search.DesCountry))
                {
                    return _flightDAO.GetAllFlightByDestinationCountry(search.DesCountry, search.FlightType);
                }
                if (!string.IsNullOrEmpty(search.OriCountry))
                {
                    return _flightDAO.GetAllFlightByOriginCountry(search.OriCountry, search.FlightType);
                }
                if (!string.IsNullOrEmpty(search.ID))
                {
                    return _flightDAO.GetAllFlightById(search.ID, search.FlightType);
                }
            }

            return searchlist;
        }
    }
}
