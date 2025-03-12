import { apiBaseUrl } from "../../../environments/development";

export const API_ENDPOINTS = {
    AUTH: {
        LOGIN: `${apiBaseUrl}/Auth/login`,
        SIGNUP: `${apiBaseUrl}/Auth/signup`,
        LOGOUT: `${apiBaseUrl}/Auth/logout`,
        REFRESH_TOKEN: `${apiBaseUrl}/Auth/refresh-token`,
    },
    USER: {
        GET_ALL: `${apiBaseUrl}/User/getAll`,
        GET_BY_ID: `${apiBaseUrl}/User/getById`,
        UPDATE: `${apiBaseUrl}/User/update`,
        DELETE: `${apiBaseUrl}/User/delete`
    },
    CATEGORY: {
        ADD: `${apiBaseUrl}/Category/add`,
        GET_ALL: `${apiBaseUrl}/Category/getAll`,
        GET_BY_ID: `${apiBaseUrl}/Category/getById`,
        UPDATE: `${apiBaseUrl}/Category/update`,
        DELETE: `${apiBaseUrl}/Category/delete`
    },
    AUTHOR: {
        ADD: `${apiBaseUrl}/Author/add`,
        GET_ALL: `${apiBaseUrl}/Author/getAll`,
        GET_BY_ID: `${apiBaseUrl}/Author/getById`,
        UPDATE: `${apiBaseUrl}/Author/update`,
        DELETE: `${apiBaseUrl}/Author/delete`
    },
    PRODUCT: {
        ADD: `${apiBaseUrl}/Product/add`,
        GET_ALL: `${apiBaseUrl}/Product/getAll`,
        GET_BY_ID: `${apiBaseUrl}/Product/getById`,
        UPDATE: `${apiBaseUrl}/Product/update`,
        DELETE: `${apiBaseUrl}/Product/delete`,
        UPLOAD_FILE: `${apiBaseUrl}/Product/uploadFile`,
        
    },
}