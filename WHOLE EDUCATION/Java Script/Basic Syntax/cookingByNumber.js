function solve(number, c1, c2, c3, c4, c5){

    let currentNumber = number;
    const array = [c1, c2, c3, c4, c5];

    for (let i = 0; i < array.length; i++) {
        
        if (array[i] == "chop")
        {
            currentNumber /= 2;
            console.log(currentNumber);
        }
        else if (array[i] == "dice")
        {
            currentNumber = Math.sqrt(currentNumber);
            console.log(currentNumber);
        }
        else if (array[i] == "spice")
        {
            currentNumber++;
            console.log(currentNumber);
        }
        else if (array[i] == "bake")
        {
            currentNumber *= 3;
            console.log(currentNumber);
        }
        else if (array[i] == "fillet")
        {
            currentNumber -= currentNumber * 0.2;
            console.log(currentNumber);
        }
    }
}
console.log(solve('32', 'chop', 'chop', 'chop', 'chop', 'chop'));
console.log(solve('9', 'dice', 'spice', 'chop', 'bake', 'fillet'));