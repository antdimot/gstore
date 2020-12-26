
const TokenManager = {
    getToken : () => localStorage.getItem('gstore_token'),
    setToken : (token) => localStorage.setItem('gstore_token', token ),
    clearToken : () => localStorage.removeItem('gstore_token'),
    hasToken : () => {
        let token = localStorage.getItem('gstore_token');
        return token != null && token.length > 0;
    }
}

export default TokenManager;