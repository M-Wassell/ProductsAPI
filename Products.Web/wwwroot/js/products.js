async function loadproductById() {
    const id = document.getElementById("productIdInput").value;
    const resultDiv = document.getElementById("productResult");

    if (!id) {
        resultDiv.innerHTML = "Please enter a product ID";
        return;
    }

    try {
        const response = await fetch(`https://localhost:7239/api/products/${id}`);

        if (!response.ok) {
            if (response.status === 404) {
                resultDiv.innerHTML = "Product not found";
                console.log(`${response.status}`);
                return;
            }
            throw new Error("API Error");
        }

        const apiResponse = await response.json();
        const product = apiResponse.data;

        resultDiv.innerHTML = `
            <h3>${product.name}</h3>
            <p><strong> Price: </strong> £${product.price}</p>
            <p><strong> Description:</strong> ${product.description}</p>`;

    }
    catch (error) {
        resultDiv.innerHTML = "Error loading product";
        console.error(error);
    }
}


function toggleSelected(row) {
    row.classList.toggle("selected");
};

async function onViewProduct(event) {
    event.stopPropagation();

    const button = event.currentTarget;
    const productId = button.dataset.productId;

    try {
        const response = await fetch(`https://localhost:7239/api/products/${productId}`);

        if (!response.ok) {
            alert("Product Not Found");
            return;
        }

        const apiResponse = await response.json();
        const product = apiResponse.data;


        document.getElementById("productModalTitle").textContent = product.name;
        document.getElementById("modalProductPrice").textContent = product.price;
        document.getElementById("modalProductDescription").textContent = product.description;

        const modal = new bootstrap.Modal(document.getElementById("productModal"));
        modal.show();

    } catch (error) {
        console.error(error);
        alert("Error loading product")
    }
}

async function deleteProduct(event) {
    event.stopPropagation();

    const button = event.currentTarget;
    const productId = button.dataset.productId;

    const confirmed = confirm("Are you sure you want to delete this product?");
    if (!confirmed) {
        return;
    }

    try {
        const response = await fetch(`https://localhost:7239/api/products/${productId}`,
            { method: "DELETE" }

        );

        if (!response.ok) {
            alert("Failed to delete product");
            return;
        }
        const row = button.closest("tr");
        row.remove();

    } catch (error) {
        console.error(error);
        alert("Error deleting product");
    }

}

async function onEditProduct(event) {
    event.stopPropagation();

    const button = event.currentTarget;
    const productId = button.dataset.productId;

    try {

        const response = await fetch(`https://localhost:7239/api/products/${productId}`);

        if (!response.ok) {
            alert("Product not found");
            return;
        }
        const apiResponse = await response.json();
        const product = apiResponse.data;

        document.getElementById("editProductId").value = productId;
        document.getElementById("editProductName").value = product.name;
        document.getElementById("editProductPrice").value = product.price;
        document.getElementById("editProductDescription").value = product.description;

        const modal = new bootstrap.Modal(
            document.getElementById("editProductModal"));

        modal.show();

    } catch (error) {
        console.error(error);
        alert("Error loading product")
    }
}

async function saveProductChanges() {
    const id = document.getElementById("editProductId").value;

    const requestBody = {
        dto: {
            name: document.getElementById("editProductName").value,
            price: Number(document.getElementById("editProductPrice").value),
            description: document.getElementById("editProductDescription").value,
        }
        
    };

    try {

        const response = await fetch(`https://localhost:7239/api/products/${id}`,
            {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(requestBody)
            }
        );
        if (!response.ok) {

            const errorText = await response.text();
            console.error("Update failed:", response.status, errorText);
            alert(`Error Updating Product (${response.status})`);

            alert("Failed to update product");
            return;
        }
        updateProductRow(id, updateProduct);

        bootstrap.Modal.getInstance(
            document.getElementById("editProductModal")
        ).hide();



    } catch (error) {
        console.error(error);
        alert("Error Updating Product");
    }
}


document.addEventListener("DOMContentLoaded", () => {

    const saveBtn = document.getElementById("saveProductBtn");
    if (saveBtn) {
        saveBtn.addEventListener("click", saveProductChanges);
    }


    document.querySelectorAll(".edit-btn")
        .forEach(button => {
            button.addEventListener("click", onEditProduct);
        })

    document.querySelectorAll(".delete-btn")
        .forEach(button => {
            button.addEventListener("click", deleteProduct);
        })

    const loadBtn = document.getElementById("loadProductBtn");
    if (loadBtn) {
        loadBtn.addEventListener("click", loadproductById);
    }

    document.querySelectorAll(".view-btn")
        .forEach(button => {
            button.addEventListener("click", onViewProduct)
        });

    const rows = document.querySelectorAll(".products-table tbody tr");
    rows.forEach(row => {
        row.addEventListener("click", () => {
            toggleSelected(row);
        })
    });
});

function updateProductRow(id, updateProduct) {

    const row = document.querySelector(
        `tr[data-product-id="${id}"]`
    );


    if (!row) {
        console.warn("Row not found for product:", id);
        return;
    }

    const cells = row.children;

    // cells[0] = Id
    cells[1].textContent = updatedProduct.name;
    cells[2].textContent = `£${updatedProduct.price}`;
    cells[3].textContent = updatedProduct.description;

}
