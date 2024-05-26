// Function to update the cart totals
function updateCart() {
    const cartItems = document.querySelectorAll('.cart-item');
    let subtotal = 0;

    const cartData = [];

    cartItems.forEach(item => {
        const quantityInput = item.querySelector('.quantity-input');
        const quantity = parseInt(quantityInput.value, 10);
        const price = parseFloat(item.querySelector('.cart-item-price').innerText.replace('$', ''));
        const productName = quantityInput.dataset.productName;

        subtotal += quantity * price;

        cartData.push({
            productName: productName,
            quantity: quantity
        });
    });

    localStorage.setItem('cartData', JSON.stringify(cartData));

    const tax = subtotal * 0.05;
    const shipping = 15.00;
    const grandTotal = subtotal + tax + shipping;

    document.getElementById('cart-subtotal').innerText = subtotal.toFixed(2);
    document.getElementById('cart-tax').innerText = tax.toFixed(2);
    document.getElementById('cart-shipping').innerText = shipping.toFixed(2);
    document.getElementById('cart-total').innerText = grandTotal.toFixed(2);
}

// Function to remove an item from the cart
function removeFromCart(productName) {
    const cartItem = document.querySelector(`.quantity-input[data-product-name="${productName}"]`).closest('.cart-item');
    cartItem.remove();
    updateCart();
}

// Function to handle checkout
function checkout() {
    window.location.href = 'Customer_Checkout.aspx';
}

function cancel() {
    window.location.href = 'Customer_Products.aspx';
}

// Function to load cart data from localStorage
function loadCartFromStorage() {
    const cartData = JSON.parse(localStorage.getItem('cartData'));
    if (cartData) {
        cartData.forEach(item => {
            const quantityInput = document.querySelector(`.quantity-input[data-product-name="${item.productName}"]`);
            if (quantityInput) {
                quantityInput.value = item.quantity;
            }
        });
    }
    updateCart();
}

// Function to attach event listeners to quantity inputs
function attachQuantityListeners() {
    const quantityInputs = document.querySelectorAll('.quantity-input');
    quantityInputs.forEach(input => {
        input.addEventListener('change', updateCart);
    });
}

// Initialize cart functionality on page load
document.addEventListener('DOMContentLoaded', () => {
    loadCartFromStorage();
    attachQuantityListeners();
});