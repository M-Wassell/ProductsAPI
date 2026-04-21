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

        const modalEl = document.getElementById("productModal");
        let modal = bootstrap.Modal.getInstance(modalEl);
        if (!modal) {
            modal = new bootstrap.Modal(modalEl);
        }
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
        const result = await response.json();
        const product = result.data;

        document.getElementById("editProductId").value = productId;
        document.getElementById("editProductName").value = product.name;
        document.getElementById("editProductPrice").value = product.price;
        document.getElementById("editProductDescription").value = product.description;
        document.getElementById("editProductStock").value = product.stockQuantity;
        document.getElementById("editProductCategory").value = product.category;

        const modalEl = document.getElementById("editProductModal");

        let modal = bootstrap.Modal.getInstance(modalEl);
        if (!modal) {
            modal = new bootstrap.Modal(modalEl);
        }
        modal.show();

    } catch (error) {
        console.error(error);
        alert("Error loading product")
    }
}

async function saveProductChanges() {
    const id = document.getElementById("editProductId").value;
    const categoryValue = document.getElementById("editProductCategory").value;

    const requestBody = {
        dto: {
                name: document.getElementById("editProductName").value,
                price: Number(document.getElementById("editProductPrice").value),
                description: document.getElementById("editProductDescription").value,
                stockQuantity: Number(document.getElementById("editProductStock").value),
                category: document.getElementById("editProductCategory").value,
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

            alert("Failed to update Product");
            return;
        }
        updateProductRow(id, requestBody.dto);

        const modalEl = document.getElementById("editProductModal");
        const modalInstance = bootstrap.Modal.getInstance(modalEl);

        if (modalInstance) {
            modalInstance.hide();
            // Fix for "Blocked aria-hidden": move focus back to body
            setTimeout(() => {
                document.body.focus();
            }, 100);
        } 

    } catch (error) {
        console.error(error);
        alert("Error Updating Product");
    }
}

async function createProduct() {
    const id = document.getElementById("editProductId").value;
    const errorsDiv = document.getElementById("createErrors");
    const successDiv = document.getElementById("successDiv");
    if (successDiv) successDiv.innerHTML = "";
    if (errorsDiv) errorsDiv.innerHTML = "";


    const newProduct = {
        CreateDto: {
            name: document.getElementById("createName").value,
            price: Number(document.getElementById("createPrice").value),
            description: document.getElementById("createDescription").value,
            stockQuantity: Number(document.getElementById("createStock").value),
            category: document.getElementById("createCategory").value
        }

    };

    try {
        const response = await fetch("https://localhost:7239/api/products", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(newProduct)
        });

        if (response.status === 400) {
            const problem = await response.json();
            console.error("Validation failed:", problem);
            showValidationErrors(problem);
            return;
        }

        if (!response.ok) {
            errorsDiv.innerText = "Error creating product"
            return;
        }

        const responseBody = await response.json();
        if (!responseBody.data) {
            errorsDiv.innerText = "No Product returned from API";
            return;

        }
        if (responseBody.data) {
            addProductRow(responseBody.data);
            // Clear the form fields after success
            document.getElementById("createProductForm").reset();
            successDiv.innerText = "Product Added Successfully";
        } else {
            errorsDiv.innerText = "No Product data returned from API";
        }

    }
    catch (error) {
        console.error(error);
        errorsDiv.innerText = "Unexpected Error creating product"

    }

}

document.addEventListener("DOMContentLoaded", () => {

    const createBtn = document.getElementById("createProductBtn");
    if (createBtn) {
        createBtn.addEventListener("click", createProduct);
    }

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

function updateProductRow(id, updatedProduct) {

    const row = document.querySelector(`tr[data-product-id="${id}"]`);

    if (!row) {
        console.warn("Row not found for product:", id);
        return;
    }

    row.querySelector('[data-field="name"]').textContent = updatedProduct.name;
    row.querySelector('[data-field="price"]').textContent = updatedProduct.price;
    row.querySelector('[data-field="description"]').textContent = updatedProduct.description;
    row.querySelector('[data-field="stockQuantity"]').textContent = updatedProduct.stockQuantity;
    row.querySelector('[data-field="category"]').textContent = updatedProduct.category;

}

function addProductRow(product) {
    const tbody = document.querySelector(".products-table tbody");

    const row = document.createElement("tr");
    row.setAttribute("data-product-id", product.id);

    row.innerHTML = `
        <td>${product.id}</td>
        <td data-field="name">${product.name}</td>
        <td data-field="price">£${product.price}</td>
        <td data-field="description">${product.description}</td>
        <td>
            <button class="btn btn-sm btn-secondary edit-btn"
                    data-product-id="${product.id}">Edit</button>
            <button class="btn btn-sm btn-danger delete-btn"
                    data-product-id="${product.id}">Delete</button>
        </td>`;

    tbody.prepend(row);

    row.querySelector(".edit-btn")
        .addEventListener("click", onEditProduct);

    row.querySelector(".delete-btn")
        .addEventListener("click", deleteProduct);
}

function clearCreateForm() {
    document.getElementById("createName").value = "";
    document.getElementById("createPrice").value = "";
    document.getElementById("createDescription").value = "";
    document.getElementById("createStock").value = "";
    document.getElementById("createCategory").value = "";
}

function showValidationErrors(problem) {
    const errorsDiv = document.getElementById("createErrors");
    errorsDiv.innerHTML = "";

    const messages = [];

    for (const field in problem.errors) {
        const feildErrors = problem.errors[field];

        feildErrors.forEach(errorMessage => {
            messages.push(errorMessage);
        })
    }
    errorsDiv.innerHTML = messages.join("<br />");
}