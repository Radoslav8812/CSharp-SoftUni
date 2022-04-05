function solve(speed, area){
    
    let limit = 0;

    if (area == "motorway"){
        limit = 130;
    }
    else if (area == "interstate"){
        limit = 90;
    }
    else if (area == "city"){
        limit = 50;
    }
    else if (area == "residential"){
        limit = 20;
    }

    const overSpeed = speed - limit;

    if (overSpeed <= 0){
        return `Driving ${speed} km/h in a ${limit} zone`;
    }

    let status = "";

    if (overSpeed <= 20){
        status = "speeding";
    }
    else if (overSpeed > 20 && overSpeed <= 40){
        status = "excessive speeding";
    }
    else{
        status = "reckless driving";
    }

    return `The speed is ${overSpeed} km/h faster than the allowed speed of ${limit} - ${status}`;
}
console.log(solve(40, 'city'));
console.log(solve(21, 'residential'));
console.log(solve(120, 'interstate'));
console.log(solve(200, 'motorway'));