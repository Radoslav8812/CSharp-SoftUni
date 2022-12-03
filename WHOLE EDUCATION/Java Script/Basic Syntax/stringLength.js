function solve(first, second, third){

    let totalLength = first.length + second.length + third.length;
    console.log(totalLength);
    
    let averageLength = Math.floor(totalLength / 3);
    console.log(averageLength);
}
solve("chocolate", "icecream", "cake");