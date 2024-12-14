import { Box, Button, Typography } from "@mui/material";
import Ticket from "../../assets/models/ticket";
import { useStore } from "../../assets/stores/store";
import { observer } from "mobx-react-lite";



interface Props {
    ticket: Ticket;
}
export default observer(function TicketSmallListItem({ ticket }: Props) {
    const { cinemaStore } = useStore();
    const session = cinemaStore.sessions.find(session => session.id === ticket.sessionId);
    const movie = cinemaStore.movies.find(movie => movie.id === session?.movieId);
    const cinema = cinemaStore.cinemas.find(cinema => cinema.id === cinemaStore.halls.find(hall => hall.id == session?.hallId)?.cinemaId);
    return (
        <>
            <Box display="flex" flexDirection="column" sx={{ backgroundColor: "white", m: "7px", p: "10px", borderRadius: "17px", boxShadow: "2px 2px 2px grey" }}>
                <Box display="flex" flexDirection="row" alignItems="center" gap="10px">
                        <Typography variant="h6" color="secondary" sx={{ textShadow: "1px 1px 1px #1a202c" }} >{movie?.title}</Typography>
                        <Typography variant="h6" color="primary">{cinema?.name}</Typography>
                </Box>
                <Box display="flex" justifyContent="space-between" alignItems="center" >
                    <Box display="flex" flexDirection="row" gap="20px" >
                        <Typography>Date {new Date(session?.date).toLocaleDateString()} at {new Date(session?.date).toLocaleTimeString()}</Typography>
                        <Typography>Seat: {ticket.seat}</Typography>
                    </Box>
                </Box>
            </Box>

        </>
    )
})