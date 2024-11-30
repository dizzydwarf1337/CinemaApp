export default interface Session {
    id: string,
    date: Date,
    movieId: string,
    hallId: string,
    ticketPrice: number,
    availibleSeats: number
}