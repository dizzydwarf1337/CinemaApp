import { Box, Button, Card, CardActions, CardContent, CardHeader,Typography } from "@mui/material";
import cinema from "../../assets/models/cinema";
import { useStore } from "../../assets/stores/store";


interface Props {
    cinema: cinema;
}

export default function cinemaListItem({cinema}:Props){

    const { userStore } = useStore();

     return (
        <>
             <Box width="100%" height="100%">
                 <Card sx={{
                     backgroundImage: cinema.imagePath ? `url(${cinema.imagePath})` : 'url(/absoluteCinema.jpg)',
                     backgroundSize: "cover",
                     backgroundPosition: "center",
                     backgroundRepeat: 'no-repeat' ,
                     height: "400px",
                     width: "310px",
                     borderRadius:"15px"
                 }}>
                     <CardHeader sx={{ color: "#f4b400", textShadow:"2px 2px #1a202c" }} title={cinema.name}></CardHeader>

                     <CardContent sx={{ color: 'white', mt: userStore.user?.role==='admin' ? "215   px" : "270px" }}>
                         <Box display="flex" flexDirection="column">
                             <Typography sx={{ color:"#f4b400", textShadow:"1px 1px  #1a202c"}}>{cinema.address}</Typography>
                         </Box>
                     </CardContent>
                     {userStore.user?.role === 'admin' ? (
                         <CardActions>
                             <Button variant="contained" color="error">Delete</Button>
                             <Button variant="contained" color="warning">Edit</Button>
                         </CardActions>
                     ) : null
                    }
                    
                </Card>
            </Box>
        </>
    )
}