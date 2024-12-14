import { observer } from "mobx-react-lite"
import { useStore } from "../../assets/stores/store";
import { Box, Button, Typography } from "@mui/material";
import { Link } from "react-router-dom";
import TicketSmallListItem from "../tickets/ticketSmallListItem";


export default observer(function HomePage() {
    const { cinemaStore, userStore } = useStore();
    return (
        <>
            <Box height="100%" display="flex" flexDirection="column" m="80px" p="20px" >
                <Box display="flex" flexDirection="row" gap="30px" alignItems="center">
                    <Box >
                    <img src="/absoluteCinema.jpg" alt="Absolute Cinema" width="700px" height="500px" style={{ borderRadius: "17px" }} />
                    </Box>

                    <Box display="flex" flexDirection="column" width="550px" height="500px">
                        <Box display="flex" flexDirection="column"  sx={{ backgroundColor: "#DEDFDF", borderRadius: "17px", width:"100%",boxShadow: "2px 2px 2px 2px grey", p: "5px" }} >
                            <Box width="100%" pl="10px">
                                <Typography variant="h5" color={userStore.getUser() ? "secondary" : "error"} sx={{ textShadow: "1px 1px 1px #1a202c" }}>{userStore.getUser() ? userStore.tickets.length>0 ? "Your future sessions" : "No future tickets found" : "Log in to see your tickets"}</Typography>
                            </Box>
                            {userStore.getUser() ?
                                <>
                                <Box width="100%" display="flex" flexDirection="column">
                                    {userStore.tickets.filter(ticket => { const session = cinemaStore.sessions.find(x => x.id === ticket.sessionId); if (session) return new Date(session.date) > new Date(); else return false; })
                                        .map(ticket => (
                                            <TicketSmallListItem ticket={ticket} key={ticket.id} />

                                    ))}
                                </Box>
                                    <Box>
                                        <Link to="/myTickets">
                                            <Button variant="contained" color="primary" size="small" sx={{ m: "7px" }}>Manage tickets</Button>
                                        </Link>
                                    </Box>
                                </>
                                :(null)
                            }
                           
                        </Box>
                        <Box>

                        </Box>
                    </Box>
                </Box>
            </Box>
        </>
    )
})