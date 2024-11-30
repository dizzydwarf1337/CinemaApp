import { createContext, useContext } from "react";
import cinemaStore from "./cinemaStore";
import userStore from "./userStore";

interface Store {
    cinemaStore:  cinemaStore;
    userStore:  userStore;
}

export const store: Store = {
    cinemaStore: new cinemaStore(),
    userStore: new userStore(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}