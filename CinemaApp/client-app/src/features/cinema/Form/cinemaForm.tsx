import { observer } from "mobx-react-lite";
import { useStore } from "../../../assets/stores/store";
import { Form, useParams } from "react-router";
import Cinema from "../../../assets/models/cinema";
import { ChangeEvent, useEffect, useState } from "react";
import { Box, Button, FormControl, Input, TextField, Typography } from "@mui/material";
import { useNavigate } from "react-router";
import CustomSnackbar from "../../../assets/layout/CustomSnackBar";

export default observer(function CinemaForm() {
    const { cinemaStore, userStore } = useStore();
    const { id } = useParams();
    const navigate = useNavigate();
    const [cinemaData, setCinemaData] = useState<Cinema>({
        id: "00000000-0000-0000-0000-000000000000",
        name: "",
        address: "",
        imagePath: "",
    });
    
    const [photo, setPhoto] = useState<File>();
    const [numOfHalls, setHalls] = useState<Number>(0);
    useEffect(() => {
        if (id) {
            const cinema = cinemaStore.cinemas.find((cinema) => cinema.id === id);
            if (cinema) setCinemaData(cinema);
        }
    }, [id, cinemaStore]);

    const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setCinemaData({ ...cinemaData, [name]: value });
    };
    const handleHallChange = (event: ChangeEvent<HTMLInputElement>) => {
        setHalls(Number(event.target.value));
    };
    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            setPhoto(event.target.files[0]);
        }
    };

    const handleSubmit = (event: ChangeEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (id) {
            cinemaStore.updateCinema(cinemaData, photo);
        } else {
            cinemaStore.createCinema(cinemaData, numOfHalls, photo);
        }
    };
    if(userStore.getUser()?.role==='admin')
        return (
        <>
            <Box display="flex" flexDirection="column" bgcolor="#DEDFDF" boxShadow="2px 2px 2px 2px grey" width="500px" height="auto" m="100px" borderRadius="10px" alignSelf="center" justifySelf="center">
                <Box display="flex" justifyContent="center" alignItems="center" mt="20px">
                    <Typography color="secondary" variant="h6" sx={{ textShadow: "1px 1px 0.1px #1a202c" }}>{id ? 'Update Cinema' : 'Create Cinema'}</Typography>
                </Box>
                    <Form onSubmit={handleSubmit}>
                        <Box display="flex" flexDirection="column" m="20px" gap="20px" height="100%" justifyContent="space-between">
                            <FormControl sx={{color:"black"} }>
                                <TextField
                                    value={cinemaData.name}
                                    name="name"
                                    onChange={handleInputChange}
                                    label="Cinema Name"
                                    required
                                />
                            </FormControl>
                            <FormControl>
                                <TextField
                                    value={cinemaData.address}
                                    name="address"
                                    onChange={handleInputChange}
                                    label="Address"
                                    required
                                />
                            </FormControl>
                            {!id? (
                                <FormControl>
                                    <TextField
                                        type="number"
                                        value={numOfHalls}
                                        onChange={handleHallChange}
                                        name="numOfHalls"
                                        label="Number Of Halls"
                                        required
                                    />
                                </FormControl>
                                ) : (null)
                            }
                            <FormControl>
                                <TextField
                                    type="file"
                                    onChange={handleFileChange}
                                    name="imagePath"
                                />
                            </FormControl>
                    
                            <FormControl>
                                <Button type="submit" variant="contained"  color={id ? "warning" : "success"}>
                                    {id ? "Update Cinema" : "Create Cinema"}
                                </Button>
                            </FormControl>
                        </Box>
                    </Form>
                </Box>
                <CustomSnackbar
                    open={cinemaStore.getCreateSnack()}
                    message={"Cinema has been created. You will be redirected"}
                    severity={"success"}
                    onClose={() => { cinemaStore.setCreateSnack(false); navigate('/cinema') }} />
                <CustomSnackbar
                    open={cinemaStore.getUpdateSnack()}
                    message={"Cinema has been updated. You will be redirected"}
                    severity={"warning"}
                    onClose={() => { cinemaStore.setUpdateSnack(false); navigate('/cinema') }} />
            </>
        );
    else return (
        <Box position="absolute" display="flex" justifyContent="center" alignItems="center" width="100%" height="100%">
            <Typography variant="h1" color="error">Forbidden</Typography>
        </Box>
            )
});
