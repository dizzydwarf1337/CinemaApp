export default interface Ticket {
    id: string,
    sessionId: string,
    seat: string,
    status: string,
    price: number,
    created: Date,
    numberOfSeats: number,
    userId:string
}