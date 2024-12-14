import { Box, Button, Card, CardContent, CardHeader,Typography } from "@mui/material";
import cinema from "../../assets/models/cinema";
import { useStore } from "../../assets/stores/store";
import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router";
import CustomSnackbar from "../../assets/layout/CustomSnackBar";


interface Props {
    cinema: cinema;
}

export default observer(function cinemaListItem({cinema}:Props){

    const { userStore,cinemaStore } = useStore();
    const navigate = useNavigate();
     return (
         <>
             <Box width="100%" height="100%" onClick={() => { navigate(`/cinema/${cinema.id}`) }} sx={{ cursor: "pointer" }}>
                 <Card sx={{
                     backgroundImage: cinema.imagePath ? `url(${cinema.imagePath})` : 'url(/absoluteCinema.jpg)',
                     backgroundSize: "cover",
                     backgroundPosition: "center",
                     backgroundRepeat: 'no-repeat',
                     height: "400px",
                     width: "310px",
                     borderRadius: "15px",
                     position: 'relative', 
                 }}>
                     <CardHeader
                         sx={{
                             position: 'absolute',
                             top: '0px', 
                             left: '10px',
                             color: "white",
                             textShadow: "2px 2px 2px red",
                            
                         }}
                         title={cinema.name}
                     />
                     <CardContent sx={{
                         position: 'absolute',
                         bottom: '0px',
                         left: '10px', 
                         right: '10px',
                         color: '#ffb400',
                     }}>
                         <Box display="flex" flexDirection="column">
                             <Typography sx={{ color: "white", textShadow: "2px 2px 2px red" }}>
                                 {cinema.address}
                             </Typography>
                         </Box>
                     </CardContent>
                 </Card>

                 {userStore.user?.role === 'admin' && (
                     <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt:"5px",mr:"7px", gap:"10px" }}>
                         <Button variant="contained" onClick={(e) => { e.stopPropagation(); navigate(`/cinema/manage/${cinema.id}`) }} color="warning">Edit</Button>
                         <Button variant="contained" onClick={(e) => { e.stopPropagation(); cinemaStore.deleteCinema(cinema.id) }} color="error">Delete</Button>
                     </Box>
                 )}
             </Box>
             <CustomSnackbar
                 message="Cinema deleted"
                 open={cinemaStore.getDeleteSnack()}
                 autoHideDuration={3000}
                 key="DeleteSnackBar"
                 severity="error"
                 onClose={() => cinemaStore.setDeleteSnack(false)}
             />
        </>
    )
})