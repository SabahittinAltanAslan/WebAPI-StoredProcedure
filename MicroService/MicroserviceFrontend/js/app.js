document.addEventListener('DOMContentLoaded', () => {
    const productApiUrl = 'http://localhost:5111/api/products';
    const orderApiUrl = 'http://localhost:5054/api/orders';
    const categoryApiUrl = 'http://localhost:5133/api/categories';
    const userApiUrl = 'http://localhost:5263/api/users';
    const loginApiUrl = 'http://localhost:5263/api/auth/login';

    async function fetchProducts() {
        try {
            const response = await fetch(productApiUrl);
            const products = await response.json();
            displayProducts(products);
        } catch (error) {
            console.error('Error fetching products:', error);
        }
    }

    function displayProducts(products) {
        const app = document.getElementById('app');
        products.forEach(product => {
            const productDiv = document.createElement('div');
            productDiv.textContent = `${product.name} - ${product.price}`;
            app.appendChild(productDiv);
        });
    }

    fetchProducts();

    const loginButton = document.getElementById('loginButton');
    loginButton.addEventListener('click', async () => {
        const username = document.getElementById('username').value;
        const password = document.getElementById('password').value;

        try {
            const response = await fetch(loginApiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ UserName: username, Password: password })
            });

            if (response.ok) {
                const data = await response.json();
                console.log('Login successful:', data);
            } else {
                console.error('Login failed');
            }
        } catch (error) {
            console.error('Error logging in:', error);
        }
    });
});
