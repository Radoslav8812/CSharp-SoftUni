function solve(type, weight, pricePerKg){

    const weightKg = weight / 1000;
    const totalPrice = weightKg * pricePerKg;

    return `I need $${totalPrice.toFixed(2)} to buy ${weightKg.toFixed(2)} kilograms ${type}.`
}
console.log(solve('orange', 2500, 1.80));
console.log(solve('apple', 1563, 2.35));
