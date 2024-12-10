import { observer } from "mobx-react-lite";
import { useStore } from "../../../assets/stores/store";
import { Box, Typography } from "@mui/material";
import { useParams } from "react-router";
import Cinema from "../../../assets/models/cinema";
import { useEffect, useState } from "react";
import SessionListItem from "../../sessions/sessionsListItem";
import dayjs from "dayjs";

export default observer(function CinemaDetails() {
    const { cinemaStore } = useStore();
    const { id } = useParams<{ id: string }>();
    const [cinema, setCinema] = useState<Cinema | null>(null);

    useEffect(() => {
        const loadCinema = async () => {
            let foundCinema = cinemaStore.cinemas.find((cinema) => cinema.id === id);
            if (!foundCinema && id) {
                foundCinema = await cinemaStore.getCinemaApi(id);
            }
            setCinema(foundCinema || null);
        };
        loadCinema();
    }, [cinemaStore, id]);

    const sessions = cinemaStore.sessions.filter((session) =>
        cinemaStore.halls.some((hall) => hall.id === session.hallId && hall.cinemaId === id) && dayjs(session.date).isAfter(dayjs())
    );

    if (!cinema) {
        return (
            <Box>
                <Typography variant="h4">Cinema not found</Typography>
            </Box>
        );
    }

    return (
        <Box display="flex" flexDirection="column"  m="100px" gap="20px">
            <Box display="flex" flexDirection="column">
                <Box display="flex" flexDirection="row" width="100%"  justifyContent="space-between">
                    <Box display="flex" flexDirection="column" justifyContent="flex-start">
                        <Box sx={{
                            borderRadius: "15px",
                            boxShadow:"2px 2px 2px grey",
                            backgroundImage: cinema.imagePath ? `url(${cinema.imagePath})` : 'url(/absoluteCinema.jpg)', width: "500px", height: "400px", backgroundSize: "cover",
                            backgroundPosition: "center",
                            backgroundRepeat: 'no-repeat',
                            }} />
                    </Box>
                    <Box display="flex" flexDirection="column"  justifyContent="center">
                        <Typography variant="h1" color="primary">{cinema.name}</Typography>
                        <Box display="flex" flexDirection="row" alignItems="center" gap="20px">
                            <Typography variant="h3" color="secondary" sx={{ textShadow:"1px 1px 1px #1a202c"} } >{cinema.address}</Typography>
                        </Box>
                    </Box>
                </Box>
            </Box>
            <Box>
                
                <Box>
                    {sessions.length > 0 ? (
                        <>
                            <Typography variant="h2">Future Sessions</Typography>
                            {sessions.map((session) => (
                                    <Box key={session.id} mb="20px">
                                        <SessionListItem session={session} />
                                    </Box>
                                ))}
                        </>
                    ) : (
                        <Typography variant="h2" color="error">No sessions available</Typography>
                    )}
                </Box>
            </Box>
        </Box>
    );
});
